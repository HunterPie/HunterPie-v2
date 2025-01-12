using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;

public class ChargeBladePhialChangeEventArgs : EventArgs
{
    public PhialChargeLevel Charge { get; }
    public int Phials { get; }
    public int MaxPhials { get; }

    public ChargeBladePhialChangeEventArgs(IChargeBlade chargeBlade)
    {
        Charge = chargeBlade.Charge;
        Phials = chargeBlade.Phials;
        MaxPhials = chargeBlade.MaxPhials;
    }
}