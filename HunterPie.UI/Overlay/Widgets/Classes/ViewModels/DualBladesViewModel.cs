using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class DualBladesViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.DualBlades;

    private bool _inDemonMode;
    public bool InDemonMode { get => _inDemonMode; set => SetValue(ref _inDemonMode, value); }

    private float _demonBuildUp;
    public float DemonBuildUp { get => _demonBuildUp; set => SetValue(ref _demonBuildUp, value); }

    private float _demonMaxBuildUp;
    public float DemonMaxBuildUp { get => _demonMaxBuildUp; set => SetValue(ref _demonMaxBuildUp, value); }

    private float _piercingBindTimer;
    public float PiercingBindTimer { get => _piercingBindTimer; set => SetValue(ref _piercingBindTimer, value); }

    private float _piercingBindMaxTimer;
    public float PiercingBindMaxTimer { get => _piercingBindMaxTimer; set => SetValue(ref _piercingBindMaxTimer, value); }

    private bool _inArchDemonMode;
    public bool InArchDemonMode { get => _inArchDemonMode; set => SetValue(ref _inArchDemonMode, value); }

    private bool _isPiercingBindVisible;
    public bool IsPiercingBindVisible { get => _isPiercingBindVisible; set => SetValue(ref _isPiercingBindVisible, value); }
}