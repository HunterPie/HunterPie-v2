using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class ChargeBladeViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.ChargeBlade;

    public ObservableCollection<ChargeBladePhialViewModel> Phials { get; } = new();

    private PhialChargeLevel _charge;
    public PhialChargeLevel Charge { get => _charge; set => SetValue(ref _charge, value); }

    private float _swordBuff;
    public float SwordBuff { get => _swordBuff; set => SetValue(ref _swordBuff, value); }

    private float _axeBuff;
    public float AxeBuff { get => _axeBuff; set => SetValue(ref _axeBuff, value); }

    private float _shieldBuff;
    public float ShieldBuff { get => _shieldBuff; set => SetValue(ref _shieldBuff, value); }

    private float _chargeBuildUp;
    public float ChargeBuildUp { get => _chargeBuildUp; set => SetValue(ref _chargeBuildUp, value); }

    private float _chargeMaxBuildUp;
    public float ChargeMaxBuildUp { get => _chargeMaxBuildUp; set => SetValue(ref _chargeMaxBuildUp, value); }
}