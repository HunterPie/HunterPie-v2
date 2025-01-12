using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.Common.Entity.Party;
public abstract class CommonPartyMember : IPartyMember, IEventDispatcher, IDisposable
{
    public abstract string Name { get; protected set; }
    public abstract int MasterRank { get; protected set; }
    public abstract int Damage { get; protected set; }
    public abstract Weapon Weapon { get; protected set; }
    public abstract int Slot { get; protected set; }
    public abstract bool IsMyself { get; protected set; }
    public abstract MemberType Type { get; protected set; }

    protected readonly SmartEvent<IPartyMember> _onDamageDealt = new();
    public event EventHandler<IPartyMember> OnDamageDealt
    {
        add => _onDamageDealt.Hook(value);
        remove => _onDamageDealt.Unhook(value);
    }

    protected readonly SmartEvent<IPartyMember> _onWeaponChange = new();
    public event EventHandler<IPartyMember> OnWeaponChange
    {
        add => _onWeaponChange.Hook(value);
        remove => _onWeaponChange.Unhook(value);
    }

    public void Dispose()
    {
        IDisposable[] events =
        {
            _onDamageDealt,
            _onWeaponChange,
        };

        events.DisposeAll();
    }
}