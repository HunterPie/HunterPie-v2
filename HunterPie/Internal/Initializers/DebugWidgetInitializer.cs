using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Features;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Metrics.View;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class DebugWidgetInitializer : IInitializer
{
    public Task Init()
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(GameProcessType.MonsterHunterRise);

        if (FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_METRICS_WIDGET))
            _ = WidgetManager.Register<TelemetricsView, TelemetricsWidgetConfig>(new TelemetricsView(config.DebugWidget));

        return Task.CompletedTask;
    }
}