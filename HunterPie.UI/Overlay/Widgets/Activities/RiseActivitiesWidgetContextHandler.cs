using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Rise;
using HunterPie.UI.Overlay.Widgets.Activities.View;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities;

public class RiseActivitiesWidgetContextHandler : IContextHandler
{
    private readonly MHRPlayer _player;
    private readonly IContextHandler[] _handlers;
    private readonly ActivitiesViewModel _viewModel;
    private readonly ActivitiesView _view;

    private readonly SubmarinesContextHandler _submarinesContextHandler;
    private readonly TrainingDojoContextHandler _trainingDojoContextHandler;
    private readonly MeowcenariesContextHandler _meowcenariesContextHandler;
    private readonly CohootContextHandler _cohootContextHandler;

    public RiseActivitiesWidgetContextHandler(MHRContext context)
    {
        _player = (MHRPlayer)context.Game.Player;

        _view = new ActivitiesView(ClientConfig.Config.Rise.Overlay.ActivitiesWidget);
        _ = WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(_view);

        _viewModel = _view.ViewModel;

        _submarinesContextHandler = new(context);
        _trainingDojoContextHandler = new(context);
        _meowcenariesContextHandler = new(context);
        _cohootContextHandler = new(context);

        _handlers = new IContextHandler[]
        {
            _submarinesContextHandler,
            _trainingDojoContextHandler,
            _meowcenariesContextHandler,
            _cohootContextHandler,
        };
        UpdateData();
    }

    public void HookEvents()
    {
        foreach (IContextHandler handler in _handlers)
            handler.HookEvents();

        _viewModel.Activities.Add(_submarinesContextHandler.ViewModel);
        _viewModel.Activities.Add(_trainingDojoContextHandler.ViewModel);
        _viewModel.Activities.Add(_meowcenariesContextHandler.ViewModel);
        _viewModel.Activities.Add(_cohootContextHandler.ViewModel);

        _player.OnStageUpdate += OnStageChange;
    }

    private void UpdateData() => _viewModel.InVisibleStage = !_player.InHuntingZone && _player.StageId != -1;

    private void OnStageChange(object sender, EventArgs e) => _viewModel.InVisibleStage = !_player.InHuntingZone && _player.StageId != -1;

    public void UnhookEvents()
    {
        foreach (IContextHandler handler in _handlers)
            handler.UnhookEvents();

        _player.OnStageUpdate -= OnStageChange;
        _ = WidgetManager.Unregister<ActivitiesView, ActivitiesWidgetConfig>(_view);
    }
}