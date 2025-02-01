using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise;

internal class CohootContextHandler : IContextHandler
{
    private readonly MHRContext _context;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;
    public readonly CohootNestViewModel ViewModel = new();

    public CohootContextHandler(MHRContext context)
    {
        _context = context;
        UpdateData();
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
        ViewModel.ElgadoCount = e.ElgadoCount;
        ViewModel.ElgadoMaxCount = e.MaxCount;
        SetGeneralCount();
    }


    private void OnKamuraCountChange(object sender, MHRCohoot e)
    {
        ViewModel.KamuraCount = e.KamuraCount;
        ViewModel.KamuraMaxCount = e.MaxCount;
        SetGeneralCount();
    }

    public void UpdateData()
    {
        ViewModel.ElgadoCount = Player.Cohoot.ElgadoCount;
        ViewModel.ElgadoMaxCount = Player.Cohoot.MaxCount;
        ViewModel.KamuraCount = Player.Cohoot.KamuraCount;
        ViewModel.KamuraMaxCount = Player.Cohoot.MaxCount;
        SetGeneralCount();
    }

    private void SetGeneralCount()
    {
        ViewModel.Count = Math.Max(ViewModel.KamuraCount, ViewModel.ElgadoCount);
        ViewModel.MaxCount = Math.Max(ViewModel.KamuraMaxCount, ViewModel.ElgadoMaxCount);
    }
}