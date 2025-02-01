using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Party;

public sealed class MHRPartyMember : CommonPartyMember, IUpdatable<MHRPartyMemberData>, IUpdatable<EntityDamageData>
{
    private int _damage;
    private Weapon _weapon;

    public override string Name { get; protected set; } = string.Empty;

    public override int Damage
    {
        get => _damage;
        protected set
        {
            if (value != _damage)
            {
                _damage = value;
                this.Dispatch(_onDamageDealt, this);
            }
        }
    }

    public override Weapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (value != _weapon)
            {
                _weapon = value;
                this.Dispatch(_onWeaponChange, this);
            }
        }
    }

    public override int Slot { get; protected set; }

    public override bool IsMyself { get; protected set; } = true;
    public override MemberType Type { get; protected set; }

    public int HighRank { get; private set; }
    public override int MasterRank { get; protected set; }

    public MHRPartyMember() { }

    public MHRPartyMember(MemberType type)
    {
        Type = type;
    }

    public void Update(MHRPartyMemberData data)
    {
        Name = data.Name;
        Weapon = data.WeaponId;
        IsMyself = data.IsMyself;
        Slot = data.Index;
        Type = data.MemberType;
        HighRank = data.HighRank;
        MasterRank = data.MasterRank;
    }

    public void Update(EntityDamageData data)
    {
        float totalDamage = data.RawDamage + data.ElementalDamage;
        Damage = (int)totalDamage;
    }

    public string GetHash() => $"{Name}:{HighRank}:{MasterRank}";
}