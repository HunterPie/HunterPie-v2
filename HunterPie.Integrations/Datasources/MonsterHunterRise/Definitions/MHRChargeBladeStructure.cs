using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRChargeBladeStructure
{
    [FieldOffset(0x9E8)] public int Phials;
    [FieldOffset(0xA08)] public float ChargeBuildUp;
    [FieldOffset(0xA28)] public float ShieldBuff;
    [FieldOffset(0xA2C)] public float SwordBuff;
}