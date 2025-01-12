using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Logger;
using HunterPie.Core.Utils;
using HunterPie.Platforms.Windows.Api.Kernel;
using System.Buffers;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace HunterPie.Platforms.Windows.Memory;

internal class WindowsMemory : IMemoryAsync
{
    private readonly IntPtr _handle;
    private readonly ArrayPool<byte> _bufferPool = ArrayPool<byte>.Shared;

    public WindowsMemory(IntPtr handle)
    {
        _handle = handle;
    }

    public async Task<string> ReadAsync(IntPtr address, int length, Encoding? encoding = null)
    {
        byte[] buffer = _bufferPool.Rent((int)length);

        await Task.Run(() => Kernel32.ReadProcessMemory(
                hProcess: _handle,
                lpBaseAddress: address,
                lpBuffer: buffer,
                dwSize: length,
                lpNumberOfBytesRead: out _
            )
        );

        string raw = (encoding ?? Encoding.UTF8).GetString(
            bytes: buffer,
            index: 0,
            count: length
        );

        _bufferPool.Return(buffer, true);
        int nullCharIndex = raw.IndexOf('\x00');

        return nullCharIndex < 0
            ? raw
            : raw[..nullCharIndex];
    }

    public async Task<T> ReadAsync<T>(IntPtr address) where T : struct => (await ReadAsync<T>(address, 1))[0];

    public Task<T[]> ReadAsync<T>(IntPtr address, int count) where T : struct
    {
        Type type = typeof(T);

        return type.IsPrimitive
            ? ReadPrimitiveAsync<T>(address, count)
            : ReadStructureAsync<T>(address, count);
    }

    public async Task<IntPtr> ReadAsync(IntPtr address, int[] offsets)
    {
        foreach (int offset in offsets)
        {
            IntPtr nextAddress = await ReadAsync<IntPtr>(address);

            if (nextAddress == IntPtr.Zero)
                return IntPtr.Zero;

            address = nextAddress + offset;
        }

        return address;
    }

    public async Task<IntPtr> ReadPtrAsync(IntPtr address, int[] offsets)
    {
        foreach (int offset in offsets)
        {
            IntPtr newAddress = address + offset;
            IntPtr nextAddress = await ReadAsync<IntPtr>(newAddress);

            if (nextAddress == IntPtr.Zero)
                return IntPtr.Zero;

            address = nextAddress;
        }

        return address;
    }

    public async Task<T> DerefAsync<T>(IntPtr address, int[] offsets) where T : struct
    {
        IntPtr valuePtr = await ReadAsync(address, offsets);

        if (valuePtr == IntPtr.Zero)
            return default;

        return await ReadAsync<T>(valuePtr);
    }

    public async Task<T> DerefPtrAsync<T>(IntPtr address, int[] offsets) where T : struct
    {
        IntPtr valuePtr = await ReadPtrAsync(address, offsets);

        if (valuePtr == IntPtr.Zero)
            return default;

        return await ReadAsync<T>(valuePtr);
    }

    public async Task WriteAsync<T>(IntPtr address, T[] data) where T : struct
    {
        byte[] buffer = MarshalHelper.StructureToBuffer(data);

        bool isSuccess = await Task.Run(() =>
            Kernel32.WriteProcessMemory(
                hProcess: _handle,
                lpBaseAddress: address,
                lpBuffer: buffer,
                nSize: buffer.Length,
                lpNumberOfBytesWritten: out _
            )
        );

        if (!isSuccess)
            throw new Win32Exception($"Failed to write to memory address '{address:P}'");
    }

    public async Task InjectAsmAsync(IntPtr address, byte[] asm)
    {
        bool hasChangedProtection = await Task.Run(() =>
            Kernel32.VirtualProtectEx(
                hProcess: _handle,
                lpAddress: address,
                dwSize: (UIntPtr)asm.Length,
                flNewProtect: 0x40,
                lpflOldProtect: out uint oldProtect
            )
        );

        if (!hasChangedProtection)
            throw new Win32Exception($"Failed to change memory protection for {asm.Length} bytes at '{address:P}'");

        await WriteAsync(address, asm);

        bool hasRestoredProtection = await Task.Run(() =>
            Kernel32.VirtualProtectEx(
                hProcess: _handle,
                lpAddress: address,
                dwSize: (UIntPtr)asm.Length,
                flNewProtect: 0x40,
                lpflOldProtect: out uint oldProtect
            )
        );

        if (!hasRestoredProtection)
            throw new Win32Exception($"Failed to restore memory protection for {asm.Length} bytes at '{address:P}'");
    }

    public async Task InjectAsync(string dll)
    {
        byte[] dllPath = Encoding.Unicode.GetBytes(dll);

        IntPtr dllNameAddress = await Task.Run(() =>
            Kernel32.VirtualAllocEx(
                hProcess: _handle,
                lpAddress: IntPtr.Zero,
                dwSize: (uint)dllPath.Length + 1,
                flAllocationType: Kernel32.AllocationType.Commit,
                flProtect: Kernel32.MemoryProtection.ExecuteReadWrite
            )
        );

        if (dllNameAddress == IntPtr.Zero)
            throw new Win32Exception("Failed to write dll path");

        await WriteAsync(dllNameAddress, dllPath);

        Log.Debug("Wrote DLL name at {0:P}", dllNameAddress);

        IntPtr kernel32Address = await Task.Run(() => Kernel32.GetModuleHandle("kernel32"));

        if (kernel32Address == IntPtr.Zero)
            throw new Win32Exception("Failed to find kernel32 address");

        Log.Debug("Found kernel32 address at {0:P}", kernel32Address);

        IntPtr loadLibraryW = await Task.Run(() => Kernel32.GetProcAddress(kernel32Address, "LoadLibraryW"));

        if (loadLibraryW == IntPtr.Zero)
            throw new Win32Exception("Failed to find LoadLibraryW address");

        Log.Debug("kernel32::LoadLibraryW -> {0:P}", loadLibraryW);

        IntPtr threadAddress = await Task.Run(() => Kernel32.CreateRemoteThread(
                hProcess: _handle,
                lpThreadAttributes: IntPtr.Zero,
                dwStackSize: 0,
                lpStartAddress: loadLibraryW,
                lpParameter: dllNameAddress,
                dwCreateFlags: 0,
                lpThreadId: IntPtr.Zero
            )
        );

        if (threadAddress == IntPtr.Zero)
            throw new Win32Exception("Failed to create remote thread");

        Log.Debug("Thread {0:P}", threadAddress);
    }

    private async Task<T[]> ReadStructureAsync<T>(IntPtr address, int count) where T : struct
    {
        int lpByteCount = Marshal.SizeOf<T>() * count;
        IntPtr bufferAddress = Marshal.AllocHGlobal(lpByteCount);

        await Task.Run(() => Kernel32.ReadProcessMemory(
                hProcess: _handle,
                lpBaseAddress: address,
                lpBuffer: bufferAddress,
                dwSize: lpByteCount,
                lpNumberOfBytesRead: out _
            )
        );

        T[] structures = MarshalHelper.BufferToStructures<T>(bufferAddress, count);

        Marshal.FreeHGlobal(bufferAddress);

        return structures;
    }

    private async Task<T[]> ReadPrimitiveAsync<T>(IntPtr address, int count) where T : struct
    {
        int lpByteCount = Marshal.SizeOf<T>() * count;
        var buffer = new T[count];

        await Task.Run(() => Kernel32.ReadProcessMemory(
                hProcess: _handle,
                lpBaseAddress: address,
                lpBuffer: buffer,
                dwSize: lpByteCount,
                lpNumberOfBytesRead: out _
            )
        );

        return buffer;
    }
}