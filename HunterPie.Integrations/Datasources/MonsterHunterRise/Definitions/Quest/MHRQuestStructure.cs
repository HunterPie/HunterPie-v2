using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHRQuestStructure
{
    [FieldOffset(0x110)]
    [MarshalAs(UnmanagedType.I4)] public QuestState State;

    /// <summary>
    /// Pointer to the current quest data structure <see cref="MHRQuestDataStructure"/>
    /// </summary>
    [FieldOffset(0x118)] public IntPtr QuestDataPointer;

    [FieldOffset(0x120)]
    [MarshalAs(UnmanagedType.I4)] public QuestType Type;

    [FieldOffset(0x15C)] public int MaxDeaths;

    [FieldOffset(0x160)] public int Deaths;

    [FieldOffset(0x170)] public float TimeElapsed;

    [FieldOffset(0x178)] public float TimeLimit;
}