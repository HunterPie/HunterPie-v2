using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWSwitchAxeStructure
{
    [FieldOffset(0x2350)] public float BuildUp;
    [FieldOffset(0x2360)] public float ChargeBuildUp;
    [FieldOffset(0x2364)] public float ChargeTimer;
}