using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHRQuestDataStructure
{
    /// <summary>
    /// Pointer to a normal quest structure <see cref="MHRNormalQuestDataStructure"/>
    /// </summary>
    [FieldOffset(0x10)] public IntPtr NormalQuestPointer;

    /// <summary>
    /// Pointer to an anomaly quest structure <see cref="MHRAnomalyQuestDataStructure"/>
    /// </summary>
    [FieldOffset(0x28)] public IntPtr AnomalyQuestPointer;

    public async Task<MHRQuestData?> GetCurrentQuestAsync(IMemoryAsync memory)
    {

        if (NormalQuestPointer != IntPtr.Zero)
        {
            MHRNormalQuestDataStructure normalQuest = await memory.ReadAsync<MHRNormalQuestDataStructure>(NormalQuestPointer);

            return new MHRQuestData
            {
                Id = normalQuest.Id,
                Level = normalQuest.Rank.ToQuestLevel(),
                Stars = normalQuest.Stars + 1
            };
        }

        if (AnomalyQuestPointer == IntPtr.Zero)
            return null;

        MHRAnomalyQuestDataStructure anomalyQuest = await memory.ReadAsync<MHRAnomalyQuestDataStructure>(AnomalyQuestPointer);

        return new MHRQuestData
        {
            Id = anomalyQuest.Id,
            Level = QuestLevel.Anomaly,
            Stars = anomalyQuest.Level
        };
    }
}