using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using System.Collections.Generic;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.Controllers;

internal class SubmarinesController : IContextHandler
{
    private readonly Dispatcher _mainDispatcher;
    private readonly MHRContext _context;
    private readonly Dictionary<MHRSubmarine, SubmarineViewModel> _submarineViewModels;
    private readonly SubmarinesViewModel _viewModel;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;


    public SubmarinesController(
        Dispatcher mainDispatcher,
        MHRContext context,
        SubmarinesViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;
        _mainDispatcher = mainDispatcher;
        _submarineViewModels = new(Player.Argosy.Submarines.Length);
    }

    public void HookEvents()
    {
        foreach (MHRSubmarine submarine in Player.Argosy.Submarines)
        {
            if (!_submarineViewModels.ContainsKey(submarine))
            {
                var submarineViewModel = new SubmarineViewModel
                {
                    Count = submarine.Count,
                    MaxCount = submarine.MaxCount,
                    DaysLeft = submarine.DaysLeft,
                    IsActive = submarine.IsUnlocked
                };

                for (int i = 0; i < submarine.MaxDays; i++)
                    submarineViewModel.Boosts.Add(
                        item: new SubmarineBoostViewModel()
                    );

                _submarineViewModels[submarine] = submarineViewModel;
            }

            submarine.OnDaysLeftChange += OnDaysLeftChange;
            submarine.OnItemCountChange += OnItemCountChange;
            submarine.OnLockStateChange += OnLockStateChange;
            UpdateBoostsData(submarine);
        }

        foreach (SubmarineViewModel vm in _submarineViewModels.Values)
            _viewModel.Submarines.Add(vm);
    }

    public void UnhookEvents()
    {
        foreach (MHRSubmarine submarine in _submarineViewModels.Keys)
        {
            submarine.OnDaysLeftChange -= OnDaysLeftChange;
            submarine.OnItemCountChange -= OnItemCountChange;
            submarine.OnLockStateChange -= OnLockStateChange;
        }

        _submarineViewModels.Clear();
        _viewModel.Submarines.Clear();
    }

    private void OnLockStateChange(object sender, MHRSubmarine e)
    {
        SubmarineViewModel vm = _submarineViewModels[e];

        vm.IsActive = e.IsUnlocked;
    }

    private void OnItemCountChange(object sender, MHRSubmarine e)
    {
        SubmarineViewModel vm = _submarineViewModels[e];

        vm.Count = e.Count;
        vm.MaxCount = e.MaxCount;
    }

    private void OnDaysLeftChange(object sender, MHRSubmarine e)
    {
        _mainDispatcher.BeginInvoke(() => UpdateBoostsData(e));
    }

    private void UpdateBoostsData(MHRSubmarine source)
    {
        SubmarineViewModel viewModel = _submarineViewModels[source];

        for (int i = 0; i < source.MaxDays; i++)
        {
            SubmarineBoostViewModel boost = viewModel.Boosts[i];
            boost.IsActive = i < source.DaysLeft;
            // TODO: Implement extra boost
            boost.IsExtraBoost = false;
        }
    }
}