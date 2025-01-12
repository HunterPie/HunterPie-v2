using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Common;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.World;
public class ArgosyContextHandler : IActivityContextHandler
{
    private readonly MHWContext _context;
    private MHWPlayer Player => (MHWPlayer)_context.Game.Player;
    private readonly ArgosyViewModel _viewModel = new();

    public IActivity ViewModel => _viewModel;

    public ArgosyContextHandler(MHWContext context)
    {
        _context = context;

        UpdateData();
    }

    public void HookEvents()
    {
        Player.Argosy.OnDaysLeftChange += OnDaysLeftChange;
    }

    public void UnhookEvents()
    {
        Player.Argosy.OnDaysLeftChange -= OnDaysLeftChange;
    }

    private void OnDaysLeftChange(object sender, MHWArgosy e)
    {
        _viewModel.IsInTown = e.IsInTown;
        _viewModel.DaysLeft = e.DaysLeft;
    }

    private void UpdateData()
    {
        _viewModel.IsInTown = Player.Argosy.IsInTown;
        _viewModel.DaysLeft = Player.Argosy.DaysLeft;
    }
}