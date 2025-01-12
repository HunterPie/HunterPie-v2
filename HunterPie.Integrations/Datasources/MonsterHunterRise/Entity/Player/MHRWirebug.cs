using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

public sealed class MHRWirebug : IEventDispatcher, IUpdatable<MHRWirebugExtrasStructure>, IUpdatable<MHRWirebugData>, IDisposable
{
    private bool _isAvailable;
    private bool _isTemporary;
    private double _timer;
    private double _maxTimer;
    private double _cooldown;
    private double _maxCooldown;
    private WirebugState _wirebugState = WirebugState.None;

    public long Address { get; internal set; }

    public bool IsAvailable
    {
        get => _isAvailable;
        private set
        {
            if (value != _isAvailable)
            {
                _isAvailable = value;
                this.Dispatch(_onAvailableChange, this);
            }
        }
    }

    public bool IsTemporary
    {
        get => _isTemporary;
        private set
        {
            if (value != _isTemporary)
            {
                _isTemporary = value;
                this.Dispatch(_onTemporaryChange, this);
            }
        }
    }

    public double Timer
    {
        get => _timer;
        private set
        {
            if (value != _timer)
            {
                _timer = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }

    public double MaxTimer
    {
        get => _maxTimer;
        private set
        {
            if (value != _maxTimer)
            {
                _maxTimer = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }

    public double Cooldown
    {
        get => _cooldown;
        private set
        {
            if (value != _cooldown)
            {
                _cooldown = value;
                this.Dispatch(_onCooldownUpdate, this);
            }
        }
    }

    public double MaxCooldown
    {
        get => _maxCooldown;
        private set
        {
            if (value != _maxCooldown)
            {
                _maxCooldown = value;
                this.Dispatch(_onCooldownUpdate, this);
            }
        }
    }

    public WirebugState WirebugState
    {
        get => _wirebugState;
        private set
        {
            if (value != _wirebugState)
            {
                _wirebugState = value;
                this.Dispatch(_onWirebugStateChange, this);
            }
        }
    }

    private readonly SmartEvent<MHRWirebug> _onAvailableChange = new();
    public event EventHandler<MHRWirebug> OnAvailableChange
    {
        add => _onAvailableChange.Hook(value);
        remove => _onAvailableChange.Unhook(value);
    }

    private readonly SmartEvent<MHRWirebug> _onTemporaryChange = new();
    public event EventHandler<MHRWirebug> OnTemporaryChange
    {
        add => _onTemporaryChange.Hook(value);
        remove => _onTemporaryChange.Unhook(value);
    }

    private readonly SmartEvent<MHRWirebug> _onTimerUpdate = new();
    public event EventHandler<MHRWirebug> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    private readonly SmartEvent<MHRWirebug> _onCooldownUpdate = new();
    public event EventHandler<MHRWirebug> OnCooldownUpdate
    {
        add => _onCooldownUpdate.Hook(value);
        remove => _onCooldownUpdate.Unhook(value);
    }

    private readonly SmartEvent<MHRWirebug> _onWirebugStateChange = new();
    public event EventHandler<MHRWirebug> OnWirebugStateChange
    {
        add => _onWirebugStateChange.Hook(value);
        remove => _onWirebugStateChange.Unhook(value);
    }

    public void Update(MHRWirebugExtrasStructure data)
    {
        Timer = data.Timer;
        MaxTimer = Timer > 0.0 ? Math.Max(MaxTimer, Timer) : 0.0;
    }

    public void Update(MHRWirebugData data)
    {
        IsAvailable = data.IsAvailable;
        IsTemporary = data.IsTemporary;
        WirebugState = data.WirebugState;
        Cooldown = data.Structure.Cooldown + data.Structure.ExtraCooldown;
        MaxCooldown = Cooldown > 0.0 ? Math.Max(MaxCooldown, data.Structure.MaxCooldown + data.Structure.ExtraCooldown) : 0.0;
    }

    public void Dispose()
    {
        IDisposable[] events = { _onAvailableChange, _onTemporaryChange, _onTimerUpdate, _onCooldownUpdate, _onWirebugStateChange };

        events.DisposeAll();
    }
}