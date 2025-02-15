using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class BuddyViewModel : Bindable
{
    private bool _isEmpty;
    private string _name;
    private int _level;

    public bool IsEmpty { get => _isEmpty; set => SetValue(ref _isEmpty, value); }
    public string Name { get => _name; set => SetValue(ref _name, value); }
    public int Level { get => _level; set => SetValue(ref _level, value); }
}