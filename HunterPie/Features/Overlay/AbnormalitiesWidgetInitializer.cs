using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Abnormality;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal class AbnormalitiesWidgetInitializer : IWidgetInitializer
{
    private readonly List<IContextHandler> _handlers = new();

    public Task LoadAsync(IContext context)
    {

        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        AbnormalityWidgetConfig[] configs = config.AbnormalityTray.Trays.Trays.ToArray();
        for (int i = 0; i < config.AbnormalityTray.Trays.Trays.Count; i++)
        {
            ref AbnormalityWidgetConfig abnormConfig = ref configs[i];

            if (!abnormConfig.Initialize)
                continue;

            _handlers.Add(new AbnormalityWidgetContextHandler(context, ref abnormConfig));
        }

        return Task.CompletedTask;
    }

    public void Unload()
    {
        foreach (IContextHandler handler in _handlers)
            handler.UnhookEvents();

        _handlers.Clear();
    }
}