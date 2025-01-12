using HunterPie.Core.Architecture;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterGlobalAilmentViewModel : ViewModel
{
    private readonly ObservableHashSet<int> _allowedAilments;

    private readonly int _id;

    public string Name { get; }

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    public MonsterGlobalAilmentViewModel(
        ObservableHashSet<int> allowedAilments,
        int id,
        string name
    )
    {
        _allowedAilments = allowedAilments;
        _id = id;
        Name = name;
    }

    public void Toggle()
    {
        IsEnabled = !IsEnabled;

        if (IsEnabled)
            _allowedAilments.Add(_id);
        else
            _allowedAilments.Remove(_id);
    }
}