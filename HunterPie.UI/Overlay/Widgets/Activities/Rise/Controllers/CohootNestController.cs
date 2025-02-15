using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.Controllers;

internal class CohootNestController : IContextHandler
{
    private readonly MHRContext _context;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;
    private readonly CohootNestsViewModel _viewModel;
    private readonly CohootNestViewModel _elgadoViewModel;
    private readonly CohootNestViewModel _kamuraViewModel;

    public CohootNestController(
        MHRContext context,
        CohootNestsViewModel viewModel,
        CohootNestViewModel elgadoViewModel,
        CohootNestViewModel kamuraViewModel)
    {
        _context = context;
        UpdateData();
        _viewModel = viewModel;
        _elgadoViewModel = elgadoViewModel;
        _kamuraViewModel = kamuraViewModel;
    }

    public void HookEvents()
    {
        Player.Cohoot.OnKamuraCountChange += OnKamuraCountChange;
        Player.Cohoot.OnElgadoCountChange += OnElgadoCountChange;
    }

    public void UnhookEvents()
    {
        Player.Cohoot.OnKamuraCountChange -= OnKamuraCountChange;
        Player.Cohoot.OnElgadoCountChange -= OnElgadoCountChange;

    }

    private void OnElgadoCountChange(object sender, MHRCohoot e)
    {
        _elgadoViewModel.SetItems(
            count: e.ElgadoCount
        );
    }

    private void OnKamuraCountChange(object sender, MHRCohoot e)
    {
        _elgadoViewModel.SetItems(
            count: e.KamuraCount
        );
    }

    public void UpdateData()
    {
        _elgadoViewModel.SetMaxItems(Player.Cohoot.MaxCount);
        _kamuraViewModel.SetMaxItems(Player.Cohoot.MaxCount);
        _viewModel.Nests.Add(_elgadoViewModel);
        _viewModel.Nests.Add(_kamuraViewModel);
    }
}