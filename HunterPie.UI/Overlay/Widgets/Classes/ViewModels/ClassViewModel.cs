using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

#nullable enable
public class ClassViewModel : ViewModel
{
    private IClassViewModel? _current;
    public IClassViewModel? Current { get => _current; set => SetValue(ref _current, value); }

    private ClassWidgetConfig? _currentSettings;
    public ClassWidgetConfig? CurrentSettings { get => _currentSettings; set => SetValue(ref _currentSettings, value); }

    private bool _inHuntingZone;
    public bool InHuntingZone { get => _inHuntingZone; set => SetValue(ref _inHuntingZone, value); }
}