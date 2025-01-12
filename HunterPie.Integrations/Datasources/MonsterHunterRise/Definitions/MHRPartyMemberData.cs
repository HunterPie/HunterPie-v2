using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

public struct MHRPartyMemberData
{
    public int Index;
    public string Name;
    public Weapon WeaponId;
    public int HighRank;
    public int MasterRank;
    public bool IsMyself;
    public MemberType MemberType;

    public MHRPartyMemberData ToPetData() => this with { Index = Index.ToPetId(), MemberType = MemberType.Pet };

    public MHRPartyMemberData ToCompanionData() => this with { MemberType = MemberType.Companion };

    public string GetHash() => $"{Name}:{HighRank}:{MasterRank}";
}