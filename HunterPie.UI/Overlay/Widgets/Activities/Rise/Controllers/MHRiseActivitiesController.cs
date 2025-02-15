using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Views;
using System;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.Controllers;

public class MHRiseActivitiesController : IContextHandler
{
    private readonly MHRContext _context;
    private readonly ActivitiesView _view;
    private readonly ActivitiesViewModel _viewModel;
    private readonly IContextHandler[] _contextHandlers;

    public MHRiseActivitiesController(
        MHRContext context,
        ActivitiesView view,
        MHRiseActivitiesViewModel activities,
        Dispatcher mainDispatcher)
    {
        _context = context;
        _view = view;
        _viewModel = view.ViewModel;
        _viewModel.Activities = activities;
        _contextHandlers = new IContextHandler[]
        {
            new CohootNestController(
                context: context,
                viewModel: activities.CohootNests,
                elgadoViewModel: DependencyContainer.Get<CohootNestViewModel>(),
                kamuraViewModel: DependencyContainer.Get<CohootNestViewModel>()
            ),
            new MeowcenariesController(context, activities.Meowcenaries),
            new SubmarinesController(mainDispatcher, context, activities.Submarines),
            new TrainingDojoController(context, activities.TrainingDojo)
        };
    }

    public void HookEvents()
    {
        _context.Game.Player.OnStageUpdate += OnStageUpdate;

        foreach (IContextHandler handler in _contextHandlers)
            handler.HookEvents();

        UpdateData();

        WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(
            widget: _view
        );
    }

    public void UnhookEvents()
    {
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;

        foreach (IContextHandler handler in _contextHandlers)
            handler.UnhookEvents();

        WidgetManager.Unregister<ActivitiesView, ActivitiesWidgetConfig>(
            widget: _view
        );
    }

    private void OnStageUpdate(object sender, EventArgs e) =>
        UpdateData();

    private void UpdateData()
    {
        _viewModel.InVisibleStage = !_context.Game.Player.InHuntingZone && _context.Game.Player.StageId != -1;
    }
}