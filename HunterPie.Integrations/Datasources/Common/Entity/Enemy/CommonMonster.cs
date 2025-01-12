using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Logger;

namespace HunterPie.Integrations.Datasources.Common.Entity.Enemy;

public abstract class CommonMonster : Scannable, IMonster, IDisposable, IEventDispatcher
{
    public abstract string Name { get; }
    public abstract int Id { get; protected set; }
    public abstract float Health { get; protected set; }
    public abstract float MaxHealth { get; protected set; }
    public abstract float Stamina { get; protected set; }
    public abstract float MaxStamina { get; protected set; }
    public abstract float CaptureThreshold { get; protected set; }
    public abstract bool IsEnraged { get; protected set; }
    public abstract Target Target { get; protected set; }
    public abstract Target ManualTarget { get; protected set; }
    public abstract IMonsterPart[] Parts { get; }
    public abstract IReadOnlyCollection<IMonsterAilment> Ailments { get; }
    public abstract IMonsterAilment Enrage { get; }
    public abstract Crown Crown { get; protected set; }
    public abstract Element[] Weaknesses { get; }
    public abstract string[] Types { get; }

    protected readonly SmartEvent<EventArgs> _onSpawn = new();
    public event EventHandler<EventArgs> OnSpawn
    {
        add => _onSpawn.Hook(value);
        remove => _onSpawn.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onLoad = new();
    public event EventHandler<EventArgs> OnLoad
    {
        add => _onLoad.Hook(value);
        remove => _onLoad.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onDespawn = new();
    public event EventHandler<EventArgs> OnDespawn
    {
        add => _onDespawn.Hook(value);
        remove => _onDespawn.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onDeath = new();
    public event EventHandler<EventArgs> OnDeath
    {
        add => _onDeath.Hook(value);
        remove => _onDeath.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onCapture = new();
    public event EventHandler<EventArgs> OnCapture
    {
        add => _onCapture.Hook(value);
        remove => _onCapture.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onCrownChange = new();
    public event EventHandler<EventArgs> OnCrownChange
    {
        add => _onCrownChange.Hook(value);
        remove => _onCrownChange.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onHealthChange = new();
    public event EventHandler<EventArgs> OnHealthChange
    {
        add => _onHealthChange.Hook(value);
        remove => _onHealthChange.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onStaminaChange = new();
    public event EventHandler<EventArgs> OnStaminaChange
    {
        add => _onStaminaChange.Hook(value);
        remove => _onStaminaChange.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onActionChange = new();
    public event EventHandler<EventArgs> OnActionChange
    {
        add => _onActionChange.Hook(value);
        remove => _onActionChange.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onEnrageStateChange = new();
    public event EventHandler<EventArgs> OnEnrageStateChange
    {
        add => _onEnrageStateChange.Hook(value);
        remove => _onEnrageStateChange.Unhook(value);
    }

    protected readonly SmartEvent<MonsterTargetEventArgs> _onTargetChange = new();
    public event EventHandler<MonsterTargetEventArgs> OnTargetChange
    {
        add => _onTargetChange.Hook(value);
        remove => _onTargetChange.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterPart> _onNewPartFound = new();
    public event EventHandler<IMonsterPart> OnNewPartFound
    {
        add => _onNewPartFound.Hook(value);
        remove => _onNewPartFound.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterAilment> _onNewAilmentFound = new();
    public event EventHandler<IMonsterAilment> OnNewAilmentFound
    {
        add => _onNewAilmentFound.Hook(value);
        remove => _onNewAilmentFound.Unhook(value);
    }

    protected readonly SmartEvent<Element[]> _onWeaknessesChange = new();
    public event EventHandler<Element[]> OnWeaknessesChange
    {
        add => _onWeaknessesChange.Hook(value);
        remove => _onWeaknessesChange.Unhook(value);
    }

    protected readonly SmartEvent<IMonster> _onCaptureThresholdChange = new();
    public event EventHandler<IMonster> OnCaptureThresholdChange
    {
        add => _onCaptureThresholdChange.Hook(value);
        remove => _onCaptureThresholdChange.Unhook(value);
    }

    protected CommonMonster(IGameProcess process) : base(process) { }

    public virtual void Dispose()
    {
        IDisposable[] events =
        {
            _onSpawn, _onLoad, _onDespawn, _onDeath, _onCapture, _onCrownChange, _onHealthChange,
            _onStaminaChange, _onActionChange, _onEnrageStateChange, _onTargetChange, _onNewPartFound,
            _onNewAilmentFound, _onWeaknessesChange, _onCaptureThresholdChange,
        };

        events.DisposeAll();

        Log.Debug("Disposing monster {0}", Name);
    }
}