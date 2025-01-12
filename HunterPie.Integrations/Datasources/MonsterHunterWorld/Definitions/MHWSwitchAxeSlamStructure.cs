using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWSwitchAxeSlamStructure
{
    [FieldOffset(0x6E5)]
    [MarshalAs(UnmanagedType.U1)] public bool IsActive;
    [FieldOffset(0x6E8)] public float Timer;
}