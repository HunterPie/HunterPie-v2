using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.Common.Entity.Enemy;
public abstract class CommonPart : IMonsterPart, IEventDispatcher, IDisposable
{
    public MonsterPartDefinition Definition { get; }
    public abstract string Id { get; protected set; }
    public abstract float Health { get; protected set; }
    public abstract float MaxHealth { get; protected set; }
    public abstract float Flinch { get; protected set; }
    public abstract float MaxFlinch { get; protected set; }
    public abstract float Sever { get; protected set; }
    public abstract float MaxSever { get; protected set; }
    public abstract float Tenderize { get; protected set; }
    public abstract float MaxTenderize { get; protected set; }
    public abstract int Count { get; protected set; }
    public abstract PartType Type { get; protected set; }

    protected CommonPart(MonsterPartDefinition definition)
    {
        Definition = definition;
    }

    protected readonly SmartEvent<IMonsterPart> _onHealthUpdate = new();
    public event EventHandler<IMonsterPart> OnHealthUpdate
    {
        add => _onHealthUpdate.Hook(value);
        remove => _onHealthUpdate.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterPart> _onBreakCountUpdate = new();
    public event EventHandler<IMonsterPart> OnBreakCountUpdate
    {
        add => _onBreakCountUpdate.Hook(value);
        remove => _onBreakCountUpdate.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterPart> _onTenderizeUpdate = new();
    public event EventHandler<IMonsterPart> OnTenderizeUpdate
    {
        add => _onTenderizeUpdate.Hook(value);
        remove => _onTenderizeUpdate.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterPart> _onFlinchUpdate = new();
    public event EventHandler<IMonsterPart> OnFlinchUpdate
    {
        add => _onFlinchUpdate.Hook(value);
        remove => _onFlinchUpdate.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterPart> _onSeverUpdate = new();
    public event EventHandler<IMonsterPart> OnSeverUpdate
    {
        add => _onSeverUpdate.Hook(value);
        remove => _onSeverUpdate.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterPart> _onPartTypeChange = new();
    public event EventHandler<IMonsterPart> OnPartTypeChange
    {
        add => _onPartTypeChange.Hook(value);
        remove => _onPartTypeChange.Unhook(value);
    }

    public virtual void Dispose()
    {
        IDisposable[] events =
        {
            _onHealthUpdate, _onBreakCountUpdate, _onTenderizeUpdate, _onFlinchUpdate,
            _onSeverUpdate, _onPartTypeChange
        };

        events.DisposeAll();
    }
}