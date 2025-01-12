using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class SwitchAxeViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.SwitchAxe;

    private float _powerBuffTimer;
    public float PowerBuffTimer { get => _powerBuffTimer; set => SetValue(ref _powerBuffTimer, value); }

    private float _maxPowerBuffTimer;
    public float MaxPowerBuffTimer { get => _maxPowerBuffTimer; set => SetValue(ref _maxPowerBuffTimer, value); }

    private float _buildUp;
    public float BuildUp { get => _buildUp; set => SetValue(ref _buildUp, value); }

    private float _maxBuildUp;
    public float MaxBuildUp { get => _maxBuildUp; set => SetValue(ref _maxBuildUp, value); }

    private float _lowBuildUp;
    public float LowBuildUp { get => _lowBuildUp; set => SetValue(ref _lowBuildUp, value); }

    private bool _isChargeActive;
    public bool IsChargeActive { get => _isChargeActive; set => SetValue(ref _isChargeActive, value); }

    private float _chargedBuildUp;
    public float ChargedBuildUp { get => _chargedBuildUp; set => SetValue(ref _chargedBuildUp, value); }

    private float _maxChargedBuildUp;
    public float MaxChargedBuildUp { get => _maxChargedBuildUp; set => SetValue(ref _maxChargedBuildUp, value); }

    private float _chargedTimer;
    public float ChargedTimer { get => _chargedTimer; set => SetValue(ref _chargedTimer, value); }

    private float _maxChargedTimer;
    public float MaxChargedTimer { get => _maxChargedTimer; set => SetValue(ref _maxChargedTimer, value); }
}