using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Vitals;
public interface IStaminaComponent
{

    public double Current { get; }
    public double Max { get; }
    public double MaxRecoverableStamina { get; }
    public double MaxPossibleStamina { get; }

    public event EventHandler<StaminaChangeEventArgs> OnStaminaChange;
}