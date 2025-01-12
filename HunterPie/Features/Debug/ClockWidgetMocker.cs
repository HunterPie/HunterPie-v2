using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture.Test;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Clock.ViewModels;
using HunterPie.UI.Overlay.Widgets.Clock.Views;
using System;

namespace HunterPie.Features.Debug;

public class ClockWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockClockWidget)
            return;

        var viewModel = new ClockViewModel
        {
            QuestTimeLeft = TimeSpan.FromSeconds(3000)
        };

        WidgetManager.Register<ClockView, ClockWidgetConfig>(
            new ClockView(new ClockWidgetConfig())
            {
                DataContext = viewModel
            }
        );

        MockBehavior.Run(() =>
        {
            if (viewModel.QuestTimeLeft.HasValue)
                viewModel.QuestTimeLeft -= TimeSpan.FromMilliseconds(16);

            viewModel.WorldTime.Add(TimeSpan.FromSeconds(1));
        }, 0.016f);
    }
}