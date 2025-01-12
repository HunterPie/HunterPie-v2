using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWDualBladesStructure
{

    [MarshalAs(UnmanagedType.U1)]
    [FieldOffset(0x2368)] public bool IsDemonModeActive;

    [MarshalAs(UnmanagedType.U1)]
    [FieldOffset(0x2369)] public bool IsArchDemonModeActive;

    [FieldOffset(0x236C)] public float DemonBuildUp;
}