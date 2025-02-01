using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Player.ViewModels;
public class WeaponSharpnessViewModel : ViewModel
{
    private Sharpness _sharpnessLevel;
    public Sharpness SharpnessLevel { get => _sharpnessLevel; set => SetValue(ref _sharpnessLevel, value); }

    private int _sharpness;
    public int Sharpness { get => _sharpness; set => SetValue(ref _sharpness, value); }

    private int _maxSharpness;
    public int MaxSharpness { get => _maxSharpness; set => SetValue(ref _maxSharpness, value); }

    private int _hitsLeft;
    public int HitsLeft { get => _hitsLeft; set => SetValue(ref _hitsLeft, value); }
}