using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Common;
using HunterPie.UI.Overlay.Widgets.Activities.View;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using HunterPie.UI.Overlay.Widgets.Activities.World;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities;
public class WorldActivitiesWidgetContextHandler : IContextHandler
{

    private readonly MHWPlayer _player;
    private readonly IActivityContextHandler[] _handlers;
    private readonly ActivitiesViewModel _viewModel;
    private readonly ActivitiesView _view;

    public WorldActivitiesWidgetContextHandler(MHWContext context)
    {
        _player = (MHWPlayer)context.Game.Player;

        _view = new ActivitiesView(ClientConfig.Config.World.Overlay.ActivitiesWidget);

        WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(_view);

        _viewModel = _view.ViewModel;

        _handlers = new IActivityContextHandler[]
        {
            new HarvestBoxContextHandler(context),
            new SteamworksContextHandler(context),
            new ArgosyContextHandler(context),
            new TailraidersContextHandler(context)
        };

        UpdateData();
    }

    public void HookEvents()
    {
        foreach (IActivityContextHandler handler in _handlers)
        {
            handler.HookEvents();
            _viewModel.Activities.Add(handler.ViewModel);
        }

        _player.OnStageUpdate += OnStageUpdate;
    }

    private void UpdateData() =>
        _viewModel.InVisibleStage = !_player.InHuntingZone && _player.ZoneId != Stage.MainMenu;

    private void OnStageUpdate(object sender, EventArgs e) =>
        _viewModel.InVisibleStage = !_player.InHuntingZone && _player.ZoneId != Stage.MainMenu;

    public void UnhookEvents()
    {
        foreach (IActivityContextHandler handler in _handlers)
            handler.UnhookEvents();

        _player.OnStageUpdate -= OnStageUpdate;

        WidgetManager.Unregister<ActivitiesView, ActivitiesWidgetConfig>(_view);
    }
}