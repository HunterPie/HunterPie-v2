using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class FertilizerDayViewModel : ViewModel
{
    private bool _isActive;
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }
}