using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

public struct MHWPartyMemberData
{
    public string Name;
    public Weapon Weapon;
    public int Damage;
    public int Slot;
    public bool IsMyself;
    public int MasterRank;
}