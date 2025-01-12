using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Events;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Classes.Controllers;

/// <summary>
/// Switch Axe widget controller
/// </summary>
internal sealed class SwitchAxeController : IClassController<SwitchAxeViewModel>
{
    private readonly IContext _context;
    private readonly ISwitchAxe _weapon;

    public SwitchAxeViewModel ViewModel { get; } = new();

    public SwitchAxeController(IContext context, ISwitchAxe weapon)
    {
        _context = context;
        _weapon = weapon;

        Update();
    }

    public void HookEvents()
    {
        _weapon.OnBuildUpChange += OnBuildUpChange;
        _weapon.OnChargeBuildUpChange += OnChargeBuildUpChange;
        _weapon.OnChargeTimerChange += OnChargeTimerChange;
        _weapon.OnSlamBuffTimerChange += OnSlamBuffTimerChange;
    }

    public void UnhookEvents()
    {
        _weapon.OnBuildUpChange -= OnBuildUpChange;
        _weapon.OnChargeBuildUpChange -= OnChargeBuildUpChange;
        _weapon.OnChargeTimerChange -= OnChargeTimerChange;
        _weapon.OnSlamBuffTimerChange -= OnSlamBuffTimerChange;
    }

    private void OnSlamBuffTimerChange(object sender, TimerChangeEventArgs e)
    {
        ViewModel.PowerBuffTimer = e.Current;
        ViewModel.MaxPowerBuffTimer = e.Max;
    }

    private void OnChargeTimerChange(object sender, TimerChangeEventArgs e)
    {
        ViewModel.IsChargeActive = true;
        ViewModel.ChargedTimer = e.Current;
        ViewModel.MaxChargedTimer = e.Max;
    }

    private void OnChargeBuildUpChange(object sender, BuildUpChangeEventArgs e)
    {
        ViewModel.IsChargeActive = false;
        ViewModel.ChargedBuildUp = e.Current;
        ViewModel.MaxChargedBuildUp = e.Max;
    }

    private void OnBuildUpChange(object sender, BuildUpChangeEventArgs e)
    {
        ViewModel.BuildUp = e.Current;
        ViewModel.MaxBuildUp = e.Max;
    }

    private void Update()
    {
        ViewModel.PowerBuffTimer = _weapon.SlamBuffTimer;
        ViewModel.MaxPowerBuffTimer = _weapon.MaxSlamBuffTimer;
        ViewModel.IsChargeActive = _weapon.ChargeTimer > 0;
        ViewModel.ChargedTimer = _weapon.ChargeTimer;
        ViewModel.MaxChargedTimer = _weapon.MaxChargeTimer;
        ViewModel.ChargedBuildUp = _weapon.ChargeBuildUp;
        ViewModel.MaxChargedBuildUp = _weapon.MaxChargeBuildUp;
        ViewModel.BuildUp = _weapon.BuildUp;
        ViewModel.MaxBuildUp = _weapon.MaxBuildUp;
        ViewModel.LowBuildUp = _weapon.LowBuildUp;
    }
}