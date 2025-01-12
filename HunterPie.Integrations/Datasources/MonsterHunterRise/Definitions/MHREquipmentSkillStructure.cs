using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHREquipmentSkillStructure
{
    [FieldOffset(0x10)] public int Id;
    [FieldOffset(0x14)] public int Level;
}