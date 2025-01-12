using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;
public class KinsectChargeChangeEventArgs : EventArgs
{
    public KinsectChargeType Type { get; }
    public float Timer { get; }

    public KinsectChargeChangeEventArgs(IInsectGlaive source)
    {
        Type = source.ChargeType;
        Timer = source.Charge;
    }
}