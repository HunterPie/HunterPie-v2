using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Monster;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal class MonsterWidgetInitializer : IWidgetInitializer
{
    private IContextHandler? _handler;

    public Task LoadAsync(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        if (!config.BossesWidget.Initialize)
            return Task.CompletedTask;

        _handler = new MonsterWidgetContextHandler(context);

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}