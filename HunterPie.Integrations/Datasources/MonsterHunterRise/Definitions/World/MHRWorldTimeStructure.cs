using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.World;

[StructLayout(LayoutKind.Explicit)]
public struct MHRWorldTimeStructure
{
    [FieldOffset(0x80)] public int Hours;
    [FieldOffset(0x84)] public int Minutes;
    [FieldOffset(0x88)] public int Seconds;
}