using HunterPie.Core.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;

public class WirebugsViewModel : Bindable
{
    private bool _isAvailable;

    public ObservableCollection<WirebugViewModel> Elements { get; } = new();
    public bool IsAvailable { get => _isAvailable; set => SetValue(ref _isAvailable, value); }
}