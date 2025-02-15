using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.Controllers;
public class TailraidersController : IContextHandler
{
    private readonly MHWContext _context;
    private MHWPlayer Player => (MHWPlayer)_context.Game.Player;
    private readonly TailraidersViewModel _viewModel;

    public TailraidersController(
        MHWContext context,
        TailraidersViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;

        UpdateData();
    }

    public void HookEvents()
    {
        Player.Tailraiders.OnQuestsLeftChange += OnQuestsLeftChange;
    }

    public void UnhookEvents()
    {
        Player.Tailraiders.OnQuestsLeftChange -= OnQuestsLeftChange;
    }

    private void OnQuestsLeftChange(object sender, MHWTailraiders e)
    {
        _viewModel.IsDeployed = e.IsDeployed;
        _viewModel.QuestsLeft = e.QuestsLeft;
    }


    private void UpdateData()
    {
        _viewModel.IsDeployed = Player.Tailraiders.IsDeployed;
        _viewModel.QuestsLeft = Player.Tailraiders.QuestsLeft;
    }
}