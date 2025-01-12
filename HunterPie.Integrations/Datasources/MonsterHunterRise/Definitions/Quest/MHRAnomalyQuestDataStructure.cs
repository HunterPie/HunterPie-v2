using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHRAnomalyQuestDataStructure
{
    [FieldOffset(0x14)] public int Id;
    [FieldOffset(0x18)] public int Level;
}