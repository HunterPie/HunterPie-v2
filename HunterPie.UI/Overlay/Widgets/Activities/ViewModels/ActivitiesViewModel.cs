using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModels;

#nullable enable
public class ActivitiesViewModel : ViewModel
{
    private bool _inVisibleStage;
    public bool InVisibleStage { get => _inVisibleStage; set => SetValue(ref _inVisibleStage, value); }

    private IActivitiesViewModel? _activities;
    public IActivitiesViewModel? Activities { get => _activities; set => SetValue(ref _activities, value); }
}