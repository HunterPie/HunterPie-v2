using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Core.Game.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
public class TailraidersViewModel : Bindable, IActivity
{
    private int _questsLeft;
    public int QuestsLeft { get => _questsLeft; set => SetValue(ref _questsLeft, value); }

    private bool _isDeployed;
    public bool IsDeployed { get => _isDeployed; set => SetValue(ref _isDeployed, value); }

    public ActivityType Type => ActivityType.Tailraiders;
}