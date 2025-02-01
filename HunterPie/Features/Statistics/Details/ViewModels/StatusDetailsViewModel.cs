using HunterPie.UI.Architecture;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.Details.ViewModels;

public class StatusDetailsViewModel : ViewModel, ISectionControllable
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private double _upTime;
    public double UpTime { get => _upTime; set => SetValue(ref _upTime, value); }

    private bool _isToggled;
    public bool IsToggled { get => _isToggled; set => SetValue(ref _isToggled, value); }

    public List<AxisSection> Activations { get; init; } = new();

    private Brush _color = Brushes.Transparent;
    public Brush Color { get => _color; set => SetValue(ref _color, value); }
}