using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class TailraidersViewModel : ViewModel
{
    private int _questsLeft;
    public int QuestsLeft { get => _questsLeft; set => SetValue(ref _questsLeft, value); }

    private bool _isDeployed;
    public bool IsDeployed { get => _isDeployed; set => SetValue(ref _isDeployed, value); }
}