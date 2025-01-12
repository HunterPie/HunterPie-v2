using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.Common.Definition;

namespace HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
public class HealthComponent : IHealthComponent, IEventDispatcher, IUpdatable<HealthData>, IDisposable
{
    private double _current;
    private double _max;
    private double _heal;
    private double _recoverableHealth;
    private double _maxPossibleHealth;

    public double Current
    {
        get => _current;
        private set
        {
            if (value != _current)
            {
                _current = value;
                this.Dispatch(_onHealthChange, new HealthChangeEventArgs(this));
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
                this.Dispatch(_onHealthChange, new HealthChangeEventArgs(this));
            }
        }
    }


    public double Heal
    {
        get => _heal;
        private set
        {
            if (value != _heal)
            {
                _heal = value;
                this.Dispatch(_onHeal, new HealthChangeEventArgs(this));
            }
        }
    }

    public double RecoverableHealth
    {
        get => _recoverableHealth;
        private set
        {
            if (value != _recoverableHealth)
            {
                _recoverableHealth = value;
                this.Dispatch(_onHealthChange, new HealthChangeEventArgs(this));
            }
        }
    }

    public double MaxPossibleHealth
    {
        get => _maxPossibleHealth;
        private set
        {
            if (value != _maxPossibleHealth)
            {
                _maxPossibleHealth = value;
                this.Dispatch(_onHealthChange, new HealthChangeEventArgs(this));
            }
        }
    }

    protected readonly SmartEvent<HealthChangeEventArgs> _onHealthChange = new();
    public event EventHandler<HealthChangeEventArgs> OnHealthChange
    {
        add => _onHealthChange.Hook(value);
        remove => _onHealthChange.Unhook(value);
    }

    protected readonly SmartEvent<HealthChangeEventArgs> _onHeal = new();
    public event EventHandler<HealthChangeEventArgs> OnHeal
    {
        add => _onHeal.Hook(value);
        remove => _onHeal.Unhook(value);
    }


    public void Update(HealthData data)
    {
        Max = data.MaxHealth;
        Current = data.Health;
        RecoverableHealth = data.RecoverableHealth;
        MaxPossibleHealth = data.MaxPossibleHealth;
        Heal = data.Heal;
    }

    public virtual void Dispose()
    {
        IDisposable[] events = { _onHealthChange, _onHeal };

        events.DisposeAll();
    }
}