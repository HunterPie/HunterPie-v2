using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRPetalaceDataStructure
{
    [FieldOffset(0x28)]
    public nint Stats;
}