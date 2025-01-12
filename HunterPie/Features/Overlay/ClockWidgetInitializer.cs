using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Clock;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

public class ClockWidgetInitializer : IWidgetInitializer
{
    private IContextHandler? _handler;

    public Task LoadAsync(IContext context)
    {
        ClockWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            (cfg) => cfg.ClockWidget
        );

        if (!config.Initialize)
            return Task.CompletedTask;

        _handler = new ClockWidgetContextHandler(
            context: context,
            configuration: config
        );

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}