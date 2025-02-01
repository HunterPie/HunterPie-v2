using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.Details.ViewModels;

public class PartyMemberDetailsViewModel : ViewModel
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private Weapon _weapon;
    public Weapon Weapon { get => _weapon; set => SetValue(ref _weapon, value); }

    private Brush _color = Brushes.Transparent;
    public Brush Color { get => _color; set => SetValue(ref _color, value); }

    private float _damage;
    public float Damage { get => _damage; set => SetValue(ref _damage, value); }

    private double _contribution;
    public double Contribution { get => _contribution; set => SetValue(ref _contribution, value); }

    private bool _isToggled;
    public bool IsToggled { get => _isToggled; set => SetValue(ref _isToggled, value); }

    public Series Damages { get; init; } = new LineSeries();

    public ObservableCollection<AbnormalityDetailsViewModel> Abnormalities { get; init; } = new();
}