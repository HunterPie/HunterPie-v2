using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.NPC;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System.Collections.Generic;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise;

internal class TrainingDojoContextHandler : IContextHandler
{
    private readonly MHRContext _context;
    private readonly Dictionary<MHRBuddy, BuddyViewModel> _buddyViewModels;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;

    public readonly TrainingDojoViewModel ViewModel = new();

    public TrainingDojoContextHandler(MHRContext context)
    {
        _context = context;
        _buddyViewModels = new(Player.TrainingDojo.Buddies.Length);

        UpdateData();
    }

    private void UpdateData()
    {
        ViewModel.Boosts = Player.TrainingDojo.Boosts;
        ViewModel.MaxBoosts = Player.TrainingDojo.MaxBoosts;
        ViewModel.Rounds = Player.TrainingDojo.Rounds;
        ViewModel.MaxRounds = Player.TrainingDojo.MaxRounds;
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
            ViewModel.Buddies.Add(vm);
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
        ViewModel.Buddies.Clear();
    }

    private void OnRoundsChange(object sender, MHRTrainingDojo e)
    {
        ViewModel.Rounds = e.Rounds;
        ViewModel.MaxRounds = e.MaxRounds;
    }

    private void OnBoostsChange(object sender, MHRTrainingDojo e)
    {
        ViewModel.Boosts = e.Boosts;
        ViewModel.MaxBoosts = e.MaxBoosts;
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