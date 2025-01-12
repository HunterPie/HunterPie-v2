using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRDualBladesStructure
{
    [FieldOffset(0x9D0)] public float DemonBuildUpMax;
    [FieldOffset(0x9E8)] public float DemonBuildUp;

    [FieldOffset(0x9EC)]
    [MarshalAs(UnmanagedType.Bool)] public bool IsDemonModeActive;

    [FieldOffset(0xA38)] public nint PiercingBindArrayPointer;
    [FieldOffset(0xA48)] public bool IsArchDemonModeActive;
}