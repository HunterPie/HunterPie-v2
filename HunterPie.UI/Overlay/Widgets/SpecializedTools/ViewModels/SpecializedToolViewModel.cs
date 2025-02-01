using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;

public class SpecializedToolViewModel : ViewModel
{
    private SpecializedToolType _id;
    private double _timer;
    private double _maxTimer;
    private double _cooldown;
    private double _maxCooldown;
    private bool _isRecharging;
    private bool _shouldBeVisible;

    public SpecializedToolType Id { get => _id; set => SetValue(ref _id, value); }
    public double Timer { get => _timer; set => SetValue(ref _timer, value); }
    public double MaxTimer { get => _maxTimer; set => SetValue(ref _maxTimer, value); }
    public double Cooldown { get => _cooldown; set => SetValue(ref _cooldown, value); }
    public double MaxCooldown { get => _maxCooldown; set => SetValue(ref _maxCooldown, value); }
    public bool IsRecharging { get => _isRecharging; set => SetValue(ref _isRecharging, value); }
    public bool ShouldBeVisible { get => _shouldBeVisible; set => SetValue(ref _shouldBeVisible, value); }
}