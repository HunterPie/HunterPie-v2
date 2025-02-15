using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Views;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.Controllers;

public class MHWorldActivitiesController : IContextHandler
{
    private readonly MHWContext _context;
    private readonly MHWPlayer _player;
    private readonly ActivitiesView _view;
    private readonly ActivitiesViewModel _viewModel;
    private readonly IContextHandler[] _contextHandlers;

    public MHWorldActivitiesController(
        MHWContext context,
        ActivitiesView view,
        MHWorldActivitiesViewModel activities)
    {
        _context = context;
        _player = context.Game.Player as MHWPlayer;
        _view = view;
        _viewModel = view.ViewModel;
        _viewModel.Activities = activities;
        _contextHandlers = new IContextHandler[]
        {
            new HarvestBoxController(context, activities.HarvestBox),
            new TailraidersController(context, activities.Tailraiders),
            new SteamworksController(context, activities.Steamworks),
            new ArgosyController(context, activities.Argosy)
        };
    }

    public void HookEvents()
    {
        _context.Game.Player.OnStageUpdate += OnStageUpdate;

        foreach (IContextHandler contextHandler in _contextHandlers)
            contextHandler.HookEvents();

        UpdateData();

        WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(
            widget: _view
        );
    }

    public void UnhookEvents()
    {
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;

        foreach (IContextHandler contextHandler in _contextHandlers)
            contextHandler.UnhookEvents();

        WidgetManager.Unregister<ActivitiesView, ActivitiesWidgetConfig>(
            widget: _view
        );
    }

    private void OnStageUpdate(object sender, EventArgs e) =>
        UpdateData();

    private void UpdateData()
    {
        _viewModel.InVisibleStage = !_player.InHuntingZone && _player.ZoneId != Stage.MainMenu;
    }
}