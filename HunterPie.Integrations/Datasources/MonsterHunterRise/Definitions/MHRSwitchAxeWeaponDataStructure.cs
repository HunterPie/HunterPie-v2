using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRSwitchAxeWeaponDataStructure
{
    [FieldOffset(0x70)] public int PhialType;
}