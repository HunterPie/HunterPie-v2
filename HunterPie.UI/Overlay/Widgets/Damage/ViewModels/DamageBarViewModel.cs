using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class DamageBarViewModel : ViewModel
{
    private Color _color;
    private double _percentage;

    public DamageBarViewModel(Color color)
    {
        _color = color;
    }

    public Color Color { get => _color; set => SetValue(ref _color, value); }
    public double Percentage { get => _percentage; set => SetValue(ref _percentage, value); }
}