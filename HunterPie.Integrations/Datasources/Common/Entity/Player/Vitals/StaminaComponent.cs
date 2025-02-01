using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.Common.Definition;

namespace HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
public class StaminaComponent : IStaminaComponent, IEventDispatcher, IUpdatable<StaminaData>, IDisposable
{
    private double _current;
    private double _max;
    private double _maxRecoverableStamina;
    private double _maxPossibleStamina;

    public double Current
    {
        get => _current;
        private set
        {
            if (value != _current)
            {
                _current = value;
                this.Dispatch(_onStaminaChange, new StaminaChangeEventArgs(this));
            }
        }
    }

    public double Max
    {
        get => _max;
        private set
        {
            if (value != _max)
            {
                _max = value;
                this.Dispatch(_onStaminaChange, new StaminaChangeEventArgs(this));
            }
        }
    }

    public double MaxRecoverableStamina
    {
        get => _maxRecoverableStamina;
        private set
        {
            if (value != _maxRecoverableStamina)
            {
                _maxRecoverableStamina = value;
                this.Dispatch(_onStaminaChange, new StaminaChangeEventArgs(this));
            }
        }
    }

    public double MaxPossibleStamina
    {
        get => _maxPossibleStamina;
        private set
        {
            if (value != _maxPossibleStamina)
            {
                _maxPossibleStamina = value;
                this.Dispatch(_onStaminaChange, new StaminaChangeEventArgs(this));
            }
        }
    }

    protected readonly SmartEvent<StaminaChangeEventArgs> _onStaminaChange = new();
    public event EventHandler<StaminaChangeEventArgs> OnStaminaChange
    {
        add => _onStaminaChange.Hook(value);
        remove => _onStaminaChange.Unhook(value);
    }

    public void Update(StaminaData data)
    {
        Max = data.MaxStamina;
        Current = data.Stamina;
        MaxRecoverableStamina = data.MaxRecoverableStamina;
        MaxPossibleStamina = data.MaxPossibleStamina;
    }

    public virtual void Dispose()
    {
        _onStaminaChange.Dispose();
    }
}