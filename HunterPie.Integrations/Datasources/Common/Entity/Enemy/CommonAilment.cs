using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Entity.Enemy;

namespace HunterPie.Integrations.Datasources.Common.Entity.Enemy;
public abstract class CommonAilment : IMonsterAilment, IEventDispatcher, IDisposable
{
    public AilmentDefinition Definition { get; }
    public abstract string Id { get; protected set; }
    public abstract int Counter { get; protected set; }
    public abstract float Timer { get; protected set; }
    public abstract float MaxTimer { get; protected set; }
    public abstract float BuildUp { get; protected set; }
    public abstract float MaxBuildUp { get; protected set; }

    protected CommonAilment(AilmentDefinition definition)
    {
        Definition = definition;
    }

    protected readonly SmartEvent<IMonsterAilment> _onTimerUpdate = new();
    public event EventHandler<IMonsterAilment> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterAilment> _onBuildUpUpdate = new();
    public event EventHandler<IMonsterAilment> OnBuildUpUpdate
    {
        add => _onBuildUpUpdate.Hook(value);
        remove => _onBuildUpUpdate.Unhook(value);
    }

    protected readonly SmartEvent<IMonsterAilment> _onCounterUpdate = new();
    public event EventHandler<IMonsterAilment> OnCounterUpdate
    {
        add => _onCounterUpdate.Hook(value);
        remove => _onCounterUpdate.Unhook(value);
    }

    public virtual void Dispose()
    {
        IDisposable[] events = { _onTimerUpdate, _onBuildUpUpdate, _onCounterUpdate, };

        events.DisposeAll();
    }
}