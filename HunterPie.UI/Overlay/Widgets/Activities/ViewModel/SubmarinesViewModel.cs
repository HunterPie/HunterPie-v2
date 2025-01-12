using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Core.Game.Enums;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class SubmarinesViewModel : Bindable, IActivity
{
    public ObservableCollection<SubmarineViewModel> Submarines { get; } = new();

    public ActivityType Type => ActivityType.Submarine;
}