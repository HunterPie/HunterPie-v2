using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Classes;

public interface IInsectGlaive : IMeleeWeapon
{
    public KinsectBuff PrimaryExtract { get; }
    public KinsectBuff SecondaryExtract { get; }
    public KinsectChargeType ChargeType { get; }
    public float AttackTimer { get; }
    public float SpeedTimer { get; }
    public float DefenseTimer { get; }
    public float Stamina { get; }
    public float MaxStamina { get; }
    public float Charge { get; }

    public event EventHandler<InsectGlaiveExtractChangeEventArgs> OnPrimaryExtractChange;
    public event EventHandler<InsectGlaiveExtractChangeEventArgs> OnSecondaryExtractChange;
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnAttackTimerChange;
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnSpeedTimerChange;
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnDefenseTimerChange;
    public event EventHandler<KinsectStaminaChangeEventArgs> OnKinsectStaminaChange;
    public event EventHandler<KinsectChargeChangeEventArgs> OnChargeChange;
}