using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Classes;

public interface IChargeBlade : IMeleeWeapon
{
    public float ShieldBuff { get; }
    public float SwordBuff { get; }
    public float AxeBuff { get; }
    public float ChargeBuildUp { get; }
    public float MaxChargeBuildUp { get; }
    public PhialChargeLevel Charge { get; }
    public int Phials { get; }
    public int MaxPhials { get; }

    public event EventHandler<ChargeBladeBuffTimerChangeEventArgs> OnShieldBuffTimerChange;
    public event EventHandler<ChargeBladeBuffTimerChangeEventArgs> OnSwordBuffTimerChange;
    public event EventHandler<ChargeBladeBuffTimerChangeEventArgs> OnAxeBuffTimerChange;
    public event EventHandler<ChargeBladeBuildUpChangeEventArgs> OnChargeBuildUpChange;
    public event EventHandler<ChargeBladePhialChangeEventArgs> OnPhialsChange;
}