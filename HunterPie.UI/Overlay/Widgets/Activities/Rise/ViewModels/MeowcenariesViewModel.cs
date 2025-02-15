using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class MeowcenariesViewModel : ViewModel
{
    private int _step;
    public int Step { get => _step; set => SetValue(ref _step, value); }

    private int _maxSteps;
    public int MaxSteps { get => _maxSteps; set => SetValue(ref _maxSteps, value); }

    private int _expectedOutcome;
    public int ExpectedOutcome { get => _expectedOutcome; set => SetValue(ref _expectedOutcome, value); }

    private int _maxOutcome;
    public int MaxOutcome { get => _maxOutcome; set => SetValue(ref _maxOutcome, value); }

    private int _buddyCount;
    public int BuddyCount { get => _buddyCount; set => SetValue(ref _buddyCount, value); }

    private int _maxBuddyCount;
    public int MaxBuddyCount { get => _maxBuddyCount; set => SetValue(ref _maxBuddyCount, value); }

    private bool _isDeployed;
    public bool IsDeployed { get => _isDeployed; set => SetValue(ref _isDeployed, value); }
}