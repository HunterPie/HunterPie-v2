using HunterPie.Core.Game.Entity.Player.Vitals;
using System;

namespace HunterPie.Core.Game.Events;
public class StaminaChangeEventArgs : EventArgs
{

    public double Stamina { get; }
    public double MaxStamina { get; }
    public double MaxRecoverableStamina { get; }
    public double MaxPossibleStamina { get; }

    public StaminaChangeEventArgs(IStaminaComponent component)
    {
        Stamina = component.Current;
        MaxStamina = component.Max;
        MaxRecoverableStamina = component.MaxRecoverableStamina;
        MaxPossibleStamina = component.MaxPossibleStamina;
    }
}