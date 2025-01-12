using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

#nullable enable
public class MonsterPartGroupViewModel : ViewModel
{
    private readonly ObservableHashSet<PartGroupType> _groups;

    public PartGroupType Type { get; }

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    public MonsterPartGroupViewModel(
        PartGroupType type,
        ObservableHashSet<PartGroupType> groups
    )
    {
        _groups = groups;
        Type = type;
    }

    public void Toggle()
    {
        IsEnabled = !IsEnabled;

        if (IsEnabled)
            _groups.Add(Type);
        else
            _groups.Remove(Type);
    }
}