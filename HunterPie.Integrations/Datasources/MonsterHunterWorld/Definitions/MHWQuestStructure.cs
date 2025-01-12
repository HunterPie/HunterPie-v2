using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWQuestStructure
{
    [FieldOffset(0x4C)] public int Id;
    /// <summary>
    /// Number of stars of this quest. 1 up to 5 is Low Rank, 6 up to 9 is High Rank and +10 is master rank.
    /// </summary>
    [FieldOffset(0x50)] public int Stars;
    [FieldOffset(0x54)]
    [MarshalAs(UnmanagedType.I4)] public QuestState State;
    [FieldOffset(0x7C)] public byte Category;
}