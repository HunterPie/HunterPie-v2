using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class ArgosyViewModel : ViewModel
{
    private int _daysLeft;
    public int DaysLeft { get => _daysLeft; set => SetValue(ref _daysLeft, value); }

    private bool _isInTown;
    public bool IsInTown { get => _isInTown; set => SetValue(ref _isInTown, value); }
}