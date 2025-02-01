namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

[Flags]
public enum QuestType : uint
{
    None = 0,
    Normal = 1,
    Kill = 1 << 1,
    Capture = 1 << 2,
    Boss = 1 << 3,
    Gather = 1 << 4,
    Expedition = 1 << 5,
    Arena = 1 << 6,
    Special = 1 << 7,
    Rampage = 1 << 8,
    Training = 1 << 9,
    Kyousei = 1 << 10,
}