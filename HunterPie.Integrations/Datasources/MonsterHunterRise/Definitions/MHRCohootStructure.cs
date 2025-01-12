using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRCohootStructure
{
    [FieldOffset(0x18)]
    public int KamuraCount;

    [FieldOffset(0x28)]
    public int ElgadoCount;
}