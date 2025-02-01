using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Activities;
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
            MHRContext ctx => new RiseActivitiesWidgetContextHandler(ctx),
            MHWContext ctx => new WorldActivitiesWidgetContextHandler(ctx),
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