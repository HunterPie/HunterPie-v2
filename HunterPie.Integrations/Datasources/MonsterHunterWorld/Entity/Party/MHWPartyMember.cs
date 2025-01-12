using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Party;

public sealed class MHWPartyMember : CommonPartyMember, IUpdatable<MHWPartyMemberData>, IUpdatable<EntityDamageData>
{
    private int _damage;
    private Weapon _weapon;
    private bool _anyNonTrivialStatisticalDamage;

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

    public override bool IsMyself { get; protected set; }

    public override MemberType Type
    {
        get => MemberType.Player;
        protected set => throw new NotSupportedException();
    }

    public override int MasterRank { get; protected set; }

    internal void ResetDamage()
    {
        _damage = 0;
        _anyNonTrivialStatisticalDamage = false;
    }

    public void Update(MHWPartyMemberData data)
    {
        Name = data.Name;
        if (data.Damage != 0)
        {
            _anyNonTrivialStatisticalDamage = true;
            Damage = data.Damage;
        }

        Weapon = data.Weapon;
        Slot = data.Slot;
        IsMyself = data.IsMyself;
        MasterRank = data.MasterRank;
    }

    public void Update(EntityDamageData data)
    {
        // If there is only trivial (zero) damage data from player statistics data,
        // we will use EntityDamageData from HunterPie.Native as a fallback.
        if (!_anyNonTrivialStatisticalDamage)
            Damage = (int)(data.RawDamage + data.ElementalDamage);
    }
}