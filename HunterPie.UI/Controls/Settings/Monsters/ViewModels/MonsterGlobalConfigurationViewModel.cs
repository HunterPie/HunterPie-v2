using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

#nullable enable
public class MonsterGlobalConfigurationViewModel : ViewModel
{
    public ObservableCollection<MonsterPartGroupViewModel> Parts { get; }

    public ObservableCollection<MonsterGlobalAilmentViewModel> Ailments { get; }

    private bool _isExpanded;
    public bool IsExpanded { get => _isExpanded; set => SetValue(ref _isExpanded, value); }

    public MonsterGlobalConfigurationViewModel(
        ObservableCollection<MonsterPartGroupViewModel> parts,
        ObservableCollection<MonsterGlobalAilmentViewModel> ailments
    )
    {
        Parts = parts;
        Ailments = ailments;
    }
}