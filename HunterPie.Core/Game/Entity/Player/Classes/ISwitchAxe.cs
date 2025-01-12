using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Classes;

public interface ISwitchAxe : IMeleeWeapon
{
    public float BuildUp { get; }
    public float MaxBuildUp { get; }
    public float LowBuildUp { get; }
    public float ChargeTimer { get; }
    public float MaxChargeTimer { get; }
    public float ChargeBuildUp { get; }
    public float MaxChargeBuildUp { get; }
    public float SlamBuffTimer { get; }
    public float MaxSlamBuffTimer { get; }

    public event EventHandler<BuildUpChangeEventArgs> OnBuildUpChange;
    public event EventHandler<TimerChangeEventArgs> OnChargeTimerChange;
    public event EventHandler<BuildUpChangeEventArgs> OnChargeBuildUpChange;
    public event EventHandler<TimerChangeEventArgs> OnSlamBuffTimerChange;
}