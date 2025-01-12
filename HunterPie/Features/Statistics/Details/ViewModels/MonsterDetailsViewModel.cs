using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.Features.Statistics.Details.ViewModels;

public class MonsterDetailsViewModel : ViewModel
{
    private bool _isInitialized;

    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private string _icon = string.Empty;
    public string Icon { get => _icon; set => SetValue(ref _icon, value); }

    private TimeSpan? _huntElapsed;
    public TimeSpan? HuntElapsed { get => _huntElapsed; set => SetValue(ref _huntElapsed, value); }

    private TimeSpan _timeElapsed;
    public TimeSpan TimeElapsed { get => _timeElapsed; set => SetValue(ref _timeElapsed, value); }

    private Crown _crown;
    public Crown Crown { get => _crown; set => SetValue(ref _crown, value); }

    private double _maxHealth;
    public double MaxHealth { get => _maxHealth; set => SetValue(ref _maxHealth, value); }

    public ObservableCollection<PartyMemberDetailsViewModel> Players { get; init; } = new();

    public ObservableCollection<AbnormalityDetailsViewModel> SelectedAbnormalities { get; init; } = new();

    public ObservableCollection<StatusDetailsViewModel> Statuses { get; init; } = new();

    public SeriesCollection DamageSeries { get; } = new();

    public SectionsCollection Sections { get; } = new();

    public Func<double, string> TimeFormatter => new((value) => TimeSpan.FromSeconds(value).ToString("mm\\:ss"));

    public Func<double, string> DamageFormatter => new((damage) => $"{damage:0.00}/s");

    public void SetupView()
    {
        EnableEnrageSections();

        foreach (PartyMemberDetailsViewModel player in Players)
            ToggleMember(player, !_isInitialized || player.IsToggled);

        SelectedAbnormalities.Clear();

        Players.Where(it => it.Abnormalities.Any())
               .SelectMany(it => it.Abnormalities)
               .ForEach(it => SelectedAbnormalities.Add(it));

        SelectedAbnormalities.Where(it => it.IsToggled)
                             .ForEach(it => ToggleSections(it, true));

        _isInitialized = true;
    }

    public void ToggleMember(PartyMemberDetailsViewModel player, bool? state = null)
    {
        player.IsToggled = state ?? !player.IsToggled;

        if (player.IsToggled)
            DamageSeries.Add(player.Damages);
        else
            DamageSeries.Remove(player.Damages);
    }

    public void ToggleSections(ISectionControllable controllable, bool? state = null)
    {
        controllable.IsToggled = state ?? !controllable.IsToggled;

        if (controllable.IsToggled)
            Sections.AddRange(controllable.Activations);
        else
            controllable.Activations.ForEach(it => Sections.Remove(it));
    }

    private void EnableEnrageSections()
    {
        StatusDetailsViewModel? enrage = Statuses.FirstOrDefault();

        if (enrage is not { })
            return;

        ToggleSections(enrage, !_isInitialized || enrage.IsToggled);
    }
}