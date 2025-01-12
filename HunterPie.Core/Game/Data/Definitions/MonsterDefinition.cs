using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Data.Definitions;

public struct MonsterDefinition
{
    public int Id;
    public int Capture;
    public bool IsNotCapturable;
    public MonsterSizeDefinition Size;
    public MonsterPartDefinition[] Parts;
    public Element[] Weaknesses;
    public string[] Types;
}