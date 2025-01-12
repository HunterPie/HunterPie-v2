using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class LongSwordViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.Longsword;

    private float _spiritGaugeRegenTimer;
    public float SpiritGaugeRegenTimer { get => _spiritGaugeRegenTimer; set => SetValue(ref _spiritGaugeRegenTimer, value); }

    private float _spiritGaugeRegenMaxTimer;
    public float SpiritGaugeRegenMaxTimer { get => _spiritGaugeRegenMaxTimer; set => SetValue(ref _spiritGaugeRegenMaxTimer, value); }

    private float _spiritGaugeBuildUp;
    public float SpiritGaugeBuildUp { get => _spiritGaugeBuildUp; set => SetValue(ref _spiritGaugeBuildUp, value); }

    private float _spiritGaugeMaxBuildUp;
    public float SpiritGaugeMaxBuildUp { get => _spiritGaugeMaxBuildUp; set => SetValue(ref _spiritGaugeMaxBuildUp, value); }

    private int _spiritLevel;
    public int SpiritLevel { get => _spiritLevel; set => SetValue(ref _spiritLevel, value); }

    private float _spiritLevelTimer;
    public float SpiritLevelTimer { get => _spiritLevelTimer; set => SetValue(ref _spiritLevelTimer, value); }

    private float _spiritLevelMaxTimer;
    public float SpiritLevelMaxTimer { get => _spiritLevelMaxTimer; set => SetValue(ref _spiritLevelMaxTimer, value); }
}