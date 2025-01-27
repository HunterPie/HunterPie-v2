﻿using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Activities.Views;
using HunterPie.UI.Overlay.Widgets.Activities.World.Controllers;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal class ActivitiesWidgetInitializer : IWidgetInitializer
{
    private IContextHandler? _handler;

    public Task LoadAsync(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        if (!config.ActivitiesWidget.Initialize)
            return Task.CompletedTask;

        _handler = context switch
        {
            MHRContext ctx => null,
            MHWContext ctx => new MHWorldActivitiesController(
                context: ctx,
                view: new ActivitiesView(config.ActivitiesWidget),
                activities: DependencyContainer.Get<MHWorldActivitiesViewModel>()
            ),
            _ => throw new NotImplementedException("unreachable")
        };

        _handler?.HookEvents();
        return Task.CompletedTask;
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}