using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Classes.Controllers;

/// <summary>
/// Dual Blades widget controller
/// </summary>
public class DualBladesController : IClassController<DualBladesViewModel>
{
    private readonly IContext _context;
    private readonly IDualBlades _weapon;

    public DualBladesViewModel ViewModel { get; } = new();

    public DualBladesController(IContext context, IDualBlades weapon)
    {
        _context = context;
        _weapon = weapon;

        Update();
    }

    public void HookEvents()
    {
        _weapon.OnDemonBuildUpChange += OnDemonBuildUpChange;
        _weapon.OnDemonModeStateChange += OnDemonModeStateChange;
        _weapon.OnPiercingBindTimerChange += OnPiercingBindTimerChange;
        _weapon.OnArchDemonModeStateChange += OnArchDemonModeStateChange;
    }

    public void UnhookEvents()
    {
        _weapon.OnDemonBuildUpChange -= OnDemonBuildUpChange;
        _weapon.OnDemonModeStateChange -= OnDemonModeStateChange;
        _weapon.OnPiercingBindTimerChange -= OnPiercingBindTimerChange;
        _weapon.OnArchDemonModeStateChange -= OnArchDemonModeStateChange;
    }

    private void OnArchDemonModeStateChange(object sender, StateChangeEventArgs<bool> e)
    {
        ViewModel.InArchDemonMode = e.State;
    }

    private void OnPiercingBindTimerChange(object sender, TimerChangeEventArgs e)
    {
        ViewModel.PiercingBindMaxTimer = e.Max;
        ViewModel.PiercingBindTimer = e.Current;
    }

    private void OnDemonModeStateChange(object sender, StateChangeEventArgs<bool> e)
    {
        ViewModel.InDemonMode = e.State;
    }

    private void OnDemonBuildUpChange(object sender, BuildUpChangeEventArgs e)
    {
        ViewModel.DemonMaxBuildUp = e.Max;
        ViewModel.DemonBuildUp = e.Current;
    }

    private void Update()
    {
        ViewModel.IsPiercingBindVisible = _context is MHRContext;
        ViewModel.InDemonMode = _weapon.IsDemonMode;
        ViewModel.PiercingBindMaxTimer = _weapon.MaxPiercingBindTimer;
        ViewModel.PiercingBindTimer = _weapon.PiercingBindTimer;
        ViewModel.DemonMaxBuildUp = _weapon.MaxDemonBuildUp;
        ViewModel.DemonBuildUp = _weapon.DemonBuildUp;
        ViewModel.InArchDemonMode = _weapon.IsArchDemonMode;
    }
}