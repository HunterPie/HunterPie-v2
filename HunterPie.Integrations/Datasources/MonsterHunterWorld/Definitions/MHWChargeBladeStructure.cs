using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWChargeBladeStructure
{
    [FieldOffset(0x2370)] public float ChargeBuildUp;
    [FieldOffset(0x2374)] public int Phials;
    [FieldOffset(0x2378)] public float ShieldBuff;
    [FieldOffset(0x237C)] public float SwordBuff;
    [FieldOffset(0x2470)] public float AxeBuff;
}