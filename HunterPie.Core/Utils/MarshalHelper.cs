using System;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Utils;

public static class MarshalHelper
{
    /// <summary>
    /// Converts a managed type into an unmanaged buffer
    /// </summary>
    /// <typeparam name="T">Type to convert</typeparam>
    /// <param name="array">Array of elements</param>
    /// <param name="size">Number of elements * sizeof(T)</param>
    /// <returns>A buffer with the data</returns>
    public static byte[] StructureToBuffer<T>(T[] array) where T : struct
    {
        int size = Marshal.SizeOf<T>() * array.Length;
        IntPtr malloced = Marshal.AllocHGlobal(size);
        byte[] buffer = new byte[size];

        for (int i = 0; i < array.Length; i++)
        {
            int offset = i * Marshal.SizeOf<T>();
            Marshal.StructureToPtr(array[i], malloced + offset, false);
        }

        Marshal.Copy(malloced, buffer, 0, size);
        Marshal.FreeHGlobal(malloced);
        return buffer;
    }

    /// <summary>
    /// Converts an unmanaged buffer to a managed structure
    /// </summary>
    /// <typeparam name="T">Structure type</typeparam>
    /// <param name="handle">Pointer to the first structure</param>
    /// <param name="count">Length</param>
    /// <returns>Array of structures</returns>
    public static T[] BufferToStructures<T>(IntPtr handle, int count)
    {
        var results = new T[count];

        for (int i = 0; i < results.Length; i++)
        {
            results[i] = Marshal.PtrToStructure<T>(IntPtr.Add(handle, i * Marshal.SizeOf<T>()));
        }

        return results;
    }
}