using HunterPie.Core.Architecture;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;

public class WirebugViewModel : Bindable
{
    private double _cooldown;
    private double _maxCooldown;
    private double _timer;
    private double _maxTimer;
    private bool _onCooldown;
    private bool _isAvailable;
    private bool _isTemporary;
    private WirebugState _wirebugState;

    public double Cooldown { get => _cooldown; set => SetValue(ref _cooldown, value); }
    public double MaxCooldown { get => _maxCooldown; set => SetValue(ref _maxCooldown, value); }
    public double Timer { get => _timer; set => SetValue(ref _timer, value); }
    public double MaxTimer { get => _maxTimer; set => SetValue(ref _maxTimer, value); }
    public bool OnCooldown { get => _onCooldown; set => SetValue(ref _onCooldown, value); }
    public bool IsAvailable { get => _isAvailable; set => SetValue(ref _isAvailable, value); }
    public bool IsTemporary { get => _isTemporary; set => SetValue(ref _isTemporary, value); }
    public WirebugState WirebugState { get => _wirebugState; set => SetValue(ref _wirebugState, value); }
}