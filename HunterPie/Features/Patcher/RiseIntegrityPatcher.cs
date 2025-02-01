using HunterPie.Core.Address.Map;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Platforms.Windows.Api.Kernel;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Patcher;

internal static class RiseIntegrityPatcher
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    /// <summary>
    /// HunterPie has assembly patches for some features to work correctly, patching in-game functions can crash the game
    /// if the integrity check is running. So we need to patch the integrity checker.
    /// If the user has REFramework then it will just skip the patches, since REFramework does that by default, since I 
    /// cannot guarantee an user has it, then HunterPie also patches it if needed.
    /// 
    /// Credits to REFramework: https://github.com/praydog/REFramework
    /// </summary>
    public static async Task Patch(IContext context)
    {
        nint[] crcFuncs =
        {
            AddressMap.GetAbsolute("CRC_FUNC_1"),
            AddressMap.GetAbsolute("CRC_FUNC_2"),
            AddressMap.GetAbsolute("CRC_FUNC_3"),
        };
        byte[][] originalAsms =
        {
            AddressMap.Get<int[]>("CRC_ORIGINAL_FUNC_1")
                .Select(e => (byte)e)
                .ToArray(),

            AddressMap.Get<int[]>("CRC_ORIGINAL_FUNC_2")
                .Select(e => (byte)e)
                .ToArray(),

            AddressMap.Get<int[]>("CRC_ORIGINAL_FUNC_3")
                .Select(e => (byte)e)
                .ToArray(),
        };
        byte[] asmPatch = { 
            // mov al, 0
            0xB0, 0x00, 
            // ret
            0xC3
        };

        for (int i = 0; i < crcFuncs.Length; i++)
        {
            nint crcFunc = crcFuncs[i];

            if (crcFunc.IsNullPointer())
                continue;

            byte[] originalAsm = originalAsms[i];

            byte[] originalInstructions = await context.Process.Memory.ReadAsync<byte>(crcFunc, 3);

            if (!originalAsm.SequenceEqual(originalInstructions))
                continue;

            await context.Process.Memory.InjectAsmAsync(crcFunc, asmPatch);

            Logger.Debug($"Patched 0x{crcFunc:X}");
        }
    }

    /// <summary>
    /// Starting with v16.0.2.0 MHRise now hooks NtProtectVirtualMemory, so we gotta patch that for HunterPie.Native to work
    /// </summary>
    public static async Task PatchProtectVirtualMemoryAsync(IContext context)
    {
        // TODO: Make this platform agnostic
        nint ntdllAddress = await Task.Run(() => Kernel32.GetModuleHandle("ntdll"));

        if (ntdllAddress.IsNullPointer())
        {
            Logger.Error("Failed to find ntdll address");
            return;
        }

        Logger.Debug($"Found ntdll address at {ntdllAddress:X}");

        nint ntProtectVirtualMemory = await Task.Run(() => Kernel32.GetProcAddress(ntdllAddress, "NtProtectVirtualMemory"));

        if (ntProtectVirtualMemory.IsNullPointer())
        {
            Logger.Error("Failed to find ntdll::NtProtectVirtualMemory address");
            return;
        }

        Logger.Debug($"Found ntdll::NtProtectVirtualMemory address at {ntProtectVirtualMemory:X}");

        byte[] originalBytes = { 0x4C, 0x8B, 0xD1, 0xB8, 0x50, 0x00, 0x00, 0x00 };
        await context.Process.Memory.InjectAsmAsync(ntProtectVirtualMemory, originalBytes);
    }
}