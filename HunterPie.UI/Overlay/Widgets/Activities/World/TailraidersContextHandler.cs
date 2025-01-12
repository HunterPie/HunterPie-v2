using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Common;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.World;
public class TailraidersContextHandler : IActivityContextHandler
{
    private readonly MHWContext _context;
    private MHWPlayer Player => (MHWPlayer)_context.Game.Player;
    private readonly TailraidersViewModel _viewModel = new();

    public IActivity ViewModel => _viewModel;

    public TailraidersContextHandler(MHWContext context)
    {
        _context = context;

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