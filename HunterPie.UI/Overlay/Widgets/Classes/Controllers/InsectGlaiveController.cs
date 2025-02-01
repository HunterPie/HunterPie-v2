using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Events;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Classes.Controllers;

/// <summary>
/// Insect Glaive widget controller
/// </summary>
public class InsectGlaiveController : IClassController<InsectGlaiveViewModel>
{
    private readonly IContext _context;
    private readonly IInsectGlaive _weapon;

    public InsectGlaiveViewModel ViewModel { get; } = new();

    public InsectGlaiveController(IContext context, IInsectGlaive weapon)
    {
        _context = context;
        _weapon = weapon;

        Update();
    }

    public void HookEvents()
    {
        _weapon.OnPrimaryExtractChange += OnPrimaryExtractChange;
        _weapon.OnSecondaryExtractChange += OnSecondaryExtractChange;
        _weapon.OnAttackTimerChange += OnAttackTimerChange;
        _weapon.OnSpeedTimerChange += OnSpeedTimerChange;
        _weapon.OnDefenseTimerChange += OnDefenseTimerChange;
        _weapon.OnKinsectStaminaChange += OnKinsectStaminaChange;
        _weapon.OnChargeChange += OnChargeChange;
    }

    public void UnhookEvents()
    {
        _weapon.OnPrimaryExtractChange -= OnPrimaryExtractChange;
        _weapon.OnSecondaryExtractChange -= OnSecondaryExtractChange;
        _weapon.OnAttackTimerChange -= OnAttackTimerChange;
        _weapon.OnSpeedTimerChange -= OnSpeedTimerChange;
        _weapon.OnDefenseTimerChange -= OnDefenseTimerChange;
        _weapon.OnKinsectStaminaChange -= OnKinsectStaminaChange;
        _weapon.OnChargeChange -= OnChargeChange;
    }

    private void OnChargeChange(object sender, KinsectChargeChangeEventArgs e)
    {
        ViewModel.ChargeType = e.Type;
        ViewModel.ChargeTimer = e.Timer;
    }

    private void OnKinsectStaminaChange(object sender, KinsectStaminaChangeEventArgs e)
    {
        ViewModel.MaxStamina = e.Max;
        ViewModel.Stamina = e.Current;
    }

    private void OnDefenseTimerChange(object sender, InsectGlaiveBuffTimerChangeEventArgs e)
    {
        ViewModel.DefenseTimer = e.Timer;
    }

    private void OnSpeedTimerChange(object sender, InsectGlaiveBuffTimerChangeEventArgs e)
    {
        ViewModel.MovementSpeedTimer = e.Timer;
    }

    private void OnAttackTimerChange(object sender, InsectGlaiveBuffTimerChangeEventArgs e)
    {
        ViewModel.AttackTimer = e.Timer;
    }

    private void OnSecondaryExtractChange(object sender, InsectGlaiveExtractChangeEventArgs e)
    {
        ViewModel.SecondaryQueuedBuff = e.Extract;
    }

    private void OnPrimaryExtractChange(object sender, InsectGlaiveExtractChangeEventArgs e)
    {
        ViewModel.PrimaryQueuedBuff = e.Extract;
    }

    private void Update()
    {
        ViewModel.Stamina = _weapon.Stamina;
        ViewModel.MaxStamina = _weapon.MaxStamina;
        ViewModel.AttackTimer = _weapon.AttackTimer;
        ViewModel.MovementSpeedTimer = _weapon.SpeedTimer;
        ViewModel.DefenseTimer = _weapon.DefenseTimer;
        ViewModel.PrimaryQueuedBuff = _weapon.PrimaryExtract;
        ViewModel.SecondaryQueuedBuff = _weapon.SecondaryExtract;
        ViewModel.ChargeTimer = _weapon.Charge;
        ViewModel.ChargeType = _weapon.ChargeType;
        ;
    }
}