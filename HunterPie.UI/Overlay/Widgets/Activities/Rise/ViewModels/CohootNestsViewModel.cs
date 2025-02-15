using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class CohootNestsViewModel : ViewModel
{
    public ObservableCollection<CohootNestViewModel> Nests { get; } = new();
}