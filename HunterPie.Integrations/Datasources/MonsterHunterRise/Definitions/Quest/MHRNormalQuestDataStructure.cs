using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHRNormalQuestDataStructure
{
    [FieldOffset(0x10)] public int Id;
    [FieldOffset(0x24)] public int Stars;

    /// <summary>
    /// 1 = Low Rank <br/>
    /// 2 = High Rank <br/>
    /// 3 = Master Rank
    /// </summary>
    [FieldOffset(0x28)] public int Rank;
}