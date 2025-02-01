using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise;

internal class MeowcenariesContextHandler : IContextHandler
{
    private readonly MHRContext _context;
    private readonly MHRPlayer _player;
    public readonly MeowcenariesViewModel ViewModel = new();

    public MeowcenariesContextHandler(MHRContext context)
    {
        _context = context;
        _player = context.Game.Player as MHRPlayer;
        UpdateData();
    }

    public void HookEvents()
    {
        _player.Meowmasters.OnDeployStateChange += OnDeployStateChange;
        _player.Meowmasters.OnStepChange += OnStepChange;
        _player.Meowmasters.OnBuddyCountChange += OnBuddyCountChange;
        _player.Meowmasters.OnExpectedOutcomeChange += OnExpectedOutcomeChange;
    }

    public void UnhookEvents()
    {
        _player.Meowmasters.OnDeployStateChange -= OnDeployStateChange;
        _player.Meowmasters.OnStepChange -= OnStepChange;
        _player.Meowmasters.OnBuddyCountChange -= OnBuddyCountChange;
        _player.Meowmasters.OnExpectedOutcomeChange -= OnExpectedOutcomeChange;
    }

    private void UpdateData()
    {
        ViewModel.Step = _player.Meowmasters.Step;
        ViewModel.MaxSteps = _player.Meowmasters.MaxSteps;
        ViewModel.ExpectedOutcome = _player.Meowmasters.ExpectedOutcome;
        ViewModel.MaxOutcome = _player.Meowmasters.MaxOutcome;
        ViewModel.BuddyCount = _player.Meowmasters.BuddyCount;
        ViewModel.MaxBuddyCount = _player.Meowmasters.MaxBuddies;
        ViewModel.IsDeployed = _player.Meowmasters.IsDeployed;
    }

    private void OnExpectedOutcomeChange(object sender, MHRMeowmasters e)
    {
        ViewModel.ExpectedOutcome = e.ExpectedOutcome;
    }

    private void OnBuddyCountChange(object sender, MHRMeowmasters e)
    {
        ViewModel.MaxBuddyCount = e.MaxBuddies;
        ViewModel.BuddyCount = e.BuddyCount;
        ViewModel.ExpectedOutcome = e.ExpectedOutcome;
    }

    private void OnStepChange(object sender, MHRMeowmasters e)
    {
        ViewModel.Step = e.Step;
        ViewModel.MaxSteps = e.MaxSteps;
    }

    private void OnDeployStateChange(object sender, MHRMeowmasters e) => ViewModel.IsDeployed = e.IsDeployed;

}