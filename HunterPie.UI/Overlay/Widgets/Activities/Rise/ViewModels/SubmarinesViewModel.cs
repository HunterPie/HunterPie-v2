using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class SubmarinesViewModel : ViewModel
{
    public ObservableCollection<SubmarineViewModel> Submarines { get; } = new();
}