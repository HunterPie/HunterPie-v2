namespace HunterPie.Core.Game.Data.Definitions;

public struct MonsterPartDefinition
{
    public int Id;
    public string String;
    public bool IsSeverable;
    public uint[] TenderizeIds;
    public PartGroupType Group;
}