using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Chat;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal class ChatWidgetInitializer : IWidgetInitializer
{
    private IContextHandler? _handler;

    public Task LoadAsync(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        if (!config.ChatWidget.Initialize)
            return Task.CompletedTask;

        if (context is MHRContext ctx)
            _handler = new ChatWidgetContextHandler(ctx);

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}