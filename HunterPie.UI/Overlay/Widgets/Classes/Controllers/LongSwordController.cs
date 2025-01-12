using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Events;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Classes.Controllers;

internal class LongSwordController : IClassController<LongSwordViewModel>
{
    private readonly ILongSword _weapon;

    public LongSwordViewModel ViewModel { get; } = new();

    public LongSwordController(ILongSword weapon)
    {
        _weapon = weapon;
    }

    public void HookEvents()
    {
        _weapon.OnSpiritBuildUpChange += OnSpiritBuildUpChange;
        _weapon.OnSpiritLevelChange += OnSpiritLevelChange;
        _weapon.OnSpiritLevelTimerChange += OnSpiritLevelTimerChange;
        _weapon.OnSpiritRegenerationChange += OnSpiritRegenerationChange;
    }

    public void UnhookEvents()
    {
        _weapon.OnSpiritBuildUpChange -= OnSpiritBuildUpChange;
        _weapon.OnSpiritLevelChange -= OnSpiritLevelChange;
        _weapon.OnSpiritLevelTimerChange -= OnSpiritLevelTimerChange;
        _weapon.OnSpiritRegenerationChange -= OnSpiritRegenerationChange;
    }

    private void OnSpiritRegenerationChange(object sender, TimerChangeEventArgs e)
    {
        ViewModel.SpiritGaugeRegenTimer = e.Current;
        ViewModel.SpiritGaugeRegenMaxTimer = e.Max;
    }

    private void OnSpiritLevelTimerChange(object sender, TimerChangeEventArgs e)
    {
        ViewModel.SpiritLevelTimer = e.Current;
        ViewModel.SpiritLevelMaxTimer = e.Max;
    }

    private void OnSpiritLevelChange(object sender, SimpleValueChangeEventArgs<int> e)
    {
        ViewModel.SpiritLevel = e.NewValue;
    }

    private void OnSpiritBuildUpChange(object sender, BuildUpChangeEventArgs e)
    {
        ViewModel.SpiritGaugeBuildUp = e.Current;
        ViewModel.SpiritGaugeMaxBuildUp = e.Max;
    }



}