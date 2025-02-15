using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.NPC;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using System.Collections.Generic;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.Controllers;

internal class TrainingDojoController : IContextHandler
{
    private readonly MHRContext _context;
    private readonly Dictionary<MHRBuddy, BuddyViewModel> _buddyViewModels;
    private readonly TrainingDojoViewModel _viewModel;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;


    public TrainingDojoController(
        MHRContext context,
        TrainingDojoViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;
        _buddyViewModels = new Dictionary<MHRBuddy, BuddyViewModel>(Player.TrainingDojo.Buddies.Length);

        UpdateData();
    }

    private void UpdateData()
    {
        _viewModel.Boosts = Player.TrainingDojo.Boosts;
        _viewModel.MaxBoosts = Player.TrainingDojo.MaxBoosts;
        _viewModel.Rounds = Player.TrainingDojo.Rounds;
        _viewModel.MaxRounds = Player.TrainingDojo.MaxRounds;
    }

    public void HookEvents()
    {
        Player.TrainingDojo.OnBoostsLeftChange += OnBoostsChange;
        Player.TrainingDojo.OnRoundsLeftChange += OnRoundsChange;

        foreach (MHRBuddy buddy in Player.TrainingDojo.Buddies)
        {
            if (_buddyViewModels.ContainsKey(buddy))
                continue;

            _buddyViewModels[buddy] = new()
            {
                Name = buddy.Name,
                Level = buddy.Level,
                IsEmpty = string.IsNullOrEmpty(buddy.Name)
            };

            buddy.OnNameChange += OnBuddyNameChange;
            buddy.OnLevelChange += OnBuddyLevelChange;
        }

        foreach (BuddyViewModel vm in _buddyViewModels.Values)
            _viewModel.Buddies.Add(vm);
    }

    public void UnhookEvents()
    {
        Player.TrainingDojo.OnBoostsLeftChange -= OnBoostsChange;
        Player.TrainingDojo.OnRoundsLeftChange -= OnRoundsChange;

        foreach (MHRBuddy buddy in _buddyViewModels.Keys)
        {
            buddy.OnNameChange -= OnBuddyNameChange;
            buddy.OnLevelChange -= OnBuddyLevelChange;
        }

        _buddyViewModels.Clear();
        _viewModel.Buddies.Clear();
    }

    private void OnRoundsChange(object sender, MHRTrainingDojo e)
    {
        _viewModel.Rounds = e.Rounds;
        _viewModel.MaxRounds = e.MaxRounds;
    }

    private void OnBoostsChange(object sender, MHRTrainingDojo e)
    {
        _viewModel.Boosts = e.Boosts;
        _viewModel.MaxBoosts = e.MaxBoosts;
    }

    private void OnBuddyNameChange(object sender, MHRBuddy e)
    {
        BuddyViewModel vm = _buddyViewModels[e];

        vm.Name = e.Name;
        vm.IsEmpty = string.IsNullOrEmpty(e.Name);
    }

    private void OnBuddyLevelChange(object sender, MHRBuddy e)
    {
        BuddyViewModel vm = _buddyViewModels[e];

        vm.Level = e.Level;
    }
}