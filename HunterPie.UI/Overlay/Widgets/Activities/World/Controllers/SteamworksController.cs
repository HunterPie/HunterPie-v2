using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.Controllers;
public class SteamworksController : IContextHandler
{

    private readonly MHWContext _context;
    private MHWPlayer Player => (MHWPlayer)_context.Game.Player;

    private readonly SteamworksViewModel _viewModel;

    public SteamworksController(
        MHWContext context,
        SteamworksViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;

        UpdateData();
    }

    public void HookEvents()
    {
        Player.Steamworks.OnNaturalFuelChange += OnNaturalFuelChange;
        Player.Steamworks.OnStoredFuelChange += OnStoredFuelChange;
    }

    public void UnhookEvents()
    {
        Player.Steamworks.OnNaturalFuelChange -= OnNaturalFuelChange;
        Player.Steamworks.OnStoredFuelChange -= OnStoredFuelChange;
    }

    private void OnStoredFuelChange(object sender, MHWSteamworks e)
    {
        _viewModel.StoredFuel = e.StoredFuel;
    }

    private void OnNaturalFuelChange(object sender, MHWSteamworks e)
    {
        _viewModel.MaxNaturalFuel = e.MaxNaturalFuel;
        _viewModel.NaturalFuel = e.NaturalFuel;
    }

    private void UpdateData()
    {
        _viewModel.StoredFuel = Player.Steamworks.StoredFuel;
        _viewModel.MaxNaturalFuel = Player.Steamworks.MaxNaturalFuel;
        _viewModel.NaturalFuel = Player.Steamworks.NaturalFuel;
    }
}