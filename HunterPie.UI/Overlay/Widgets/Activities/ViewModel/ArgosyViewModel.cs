using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Core.Game.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class ArgosyViewModel : Bindable, IActivity
{
    private int _daysLeft;
    public int DaysLeft { get => _daysLeft; set => SetValue(ref _daysLeft, value); }

    private bool _isInTown;
    public bool IsInTown { get => _isInTown; set => SetValue(ref _isInTown, value); }

    public ActivityType Type => ActivityType.Argosy;
}