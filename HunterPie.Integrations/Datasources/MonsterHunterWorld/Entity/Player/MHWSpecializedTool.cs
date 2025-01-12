using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;

public class MHWSpecializedTool : ISpecializedTool, IEventDispatcher, IUpdatable<MHWSpecializedToolStructure>, IDisposable
{
    private SpecializedToolType _id;
    private float _cooldown;
    private float _timer;

    public SpecializedToolType Id
    {
        get => _id;
        set
        {
            if (value != _id)
            {
                _id = value;
                this.Dispatch(_onChange, this);
            }
        }
    }

    public float Cooldown
    {
        get => _cooldown;
        set
        {
            if (value != _cooldown)
            {
                _cooldown = value;
                this.Dispatch(_onCooldownUpdate, this);
            }
        }
    }

    public float MaxCooldown { get; private set; }

    public float Timer
    {
        get => _timer;
        set
        {
            if (value != _timer)
            {
                _timer = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }

    public float MaxTimer { get; private set; }

    private readonly SmartEvent<ISpecializedTool> _onCooldownUpdate = new();
    public event EventHandler<ISpecializedTool> OnCooldownUpdate
    {
        add => _onCooldownUpdate.Hook(value);
        remove => _onCooldownUpdate.Unhook(value);
    }

    private readonly SmartEvent<ISpecializedTool> _onTimerUpdate = new();
    public event EventHandler<ISpecializedTool> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    private readonly SmartEvent<ISpecializedTool> _onChange = new();
    public event EventHandler<ISpecializedTool> OnChange
    {
        add => _onChange.Hook(value);
        remove => _onChange.Unhook(value);
    }

    public void Update(MHWSpecializedToolStructure data)
    {
        Id = data.Id;
        MaxTimer = data.MaxTimer;
        Timer = data.Timer;
        MaxCooldown = data.MaxCooldown;
        Cooldown = data.Cooldown;
    }

    public void Dispose()
    {
        _onCooldownUpdate.Dispose();
        _onTimerUpdate.Dispose();
        _onChange.Dispose();
    }
}