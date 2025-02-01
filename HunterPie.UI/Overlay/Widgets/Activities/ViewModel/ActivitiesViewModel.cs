using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Entity.Environment;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class ActivitiesViewModel : Bindable
{
    private bool _inVisibleStage;

    public bool InVisibleStage { get => _inVisibleStage; set => SetValue(ref _inVisibleStage, value); }
    public ObservableCollection<IActivity> Activities { get; } = new();
}