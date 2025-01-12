using HunterPie.UI.Architecture;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.Details.ViewModels;

public class AbnormalityDetailsViewModel : ViewModel, ISectionControllable
{
    private string _name;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private string _icon;
    public string Icon { get => _icon; set => SetValue(ref _icon, value); }

    private double _upTime;
    public double UpTime { get => _upTime; set => SetValue(ref _upTime, value); }

    private bool _isToggled;
    public bool IsToggled { get => _isToggled; set => SetValue(ref _isToggled, value); }

    public List<AxisSection> Activations { get; init; } = new();

    private Brush _color;
    public Brush Color { get => _color; set => SetValue(ref _color, value); }
}