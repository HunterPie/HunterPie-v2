using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.Common.Entity.Player;
public abstract class CommonAbnormality : IAbnormality, IEventDispatcher, IDisposable
{
    public abstract string Id { get; protected set; }
    public abstract string Name { get; protected set; }
    public abstract string Icon { get; protected set; }
    public abstract AbnormalityType Type { get; protected set; }
    public abstract float Timer { get; protected set; }
    public abstract float MaxTimer { get; protected set; }
    public abstract bool IsInfinite { get; protected set; }
    public abstract int Level { get; protected set; }
    public abstract bool IsBuildup { get; protected set; }

    protected readonly SmartEvent<IAbnormality> _onTimerUpdate = new();
    public event EventHandler<IAbnormality> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    public void Dispose()
    {
        _onTimerUpdate.Dispose();
    }
}