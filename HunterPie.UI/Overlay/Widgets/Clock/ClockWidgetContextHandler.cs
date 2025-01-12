using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.UI.Overlay.Widgets.Clock.ViewModels;
using HunterPie.UI.Overlay.Widgets.Clock.Views;
using System;

namespace HunterPie.UI.Overlay.Widgets.Clock;

public class ClockWidgetContextHandler : IContextHandler
{
    private readonly ClockViewModel _viewModel;
    private readonly ClockView _view;
    private readonly IContext _context;

    public ClockWidgetContextHandler(
        IContext context,
        ClockWidgetConfig configuration
    )
    {
        _view = new ClockView(configuration);
        _viewModel = _view.ViewModel;
        _context = context;

        WidgetManager.Register<ClockView, ClockWidgetConfig>(
            widget: _view
        );

        HookEvents();
        UpdateData();
    }

    public void HookEvents()
    {
        _context.Game.OnWorldTimeChange += OnWorldTimeChange;
        _context.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;

        if (_context.Game.Quest is not { } quest)
            return;

        quest.OnTimeLeftChange += OnQuestTimeLeftChange;
    }

    public void UnhookEvents()
    {
        WidgetManager.Unregister<ClockView, ClockWidgetConfig>(
            widget: _view
        );

        _context.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;
        _context.Game.OnWorldTimeChange -= OnWorldTimeChange;

        if (_context.Game.Quest is not { } quest)
            return;

        quest.OnTimeLeftChange -= OnQuestTimeLeftChange;
    }

    private void OnQuestEnd(object sender, QuestEndEventArgs e)
    {
        e.Quest.OnTimeLeftChange -= OnQuestTimeLeftChange;

        _viewModel.QuestTimeLeft = null;
    }

    private void OnQuestStart(object sender, IQuest e)
    {
        _viewModel.QuestTimeLeft = e.TimeLeft;

        e.OnTimeLeftChange += OnQuestTimeLeftChange;
    }

    private void UpdateData()
    {
        _viewModel.WorldTime = _context.Game.WorldTime;
        _viewModel.QuestTimeLeft = _context.Game.Quest?.TimeLeft;
    }

    private void OnWorldTimeChange(object sender, SimpleValueChangeEventArgs<TimeOnly> e)
    {
        _viewModel.WorldTime = e.NewValue;
    }

    private void OnQuestTimeLeftChange(object sender, SimpleValueChangeEventArgs<TimeSpan> e)
    {
        _viewModel.QuestTimeLeft = e.NewValue;
    }
}