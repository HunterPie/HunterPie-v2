using HunterPie.Core.Game.Entity.Player.Classes;
using System;

namespace HunterPie.Core.Game.Events;
public class KinsectStaminaChangeEventArgs : EventArgs
{
    public float Current { get; }
    public float Max { get; }

    public KinsectStaminaChangeEventArgs(IInsectGlaive source)
    {
        Current = source.Stamina;
        Max = source.MaxStamina;
    }
}