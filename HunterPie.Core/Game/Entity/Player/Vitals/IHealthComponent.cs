using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Vitals;
public interface IHealthComponent
{

    public double Current { get; }
    public double Max { get; }
    public double Heal { get; }
    public double RecoverableHealth { get; }
    public double MaxPossibleHealth { get; }

    public event EventHandler<HealthChangeEventArgs> OnHealthChange;
    public event EventHandler<HealthChangeEventArgs> OnHeal;
}