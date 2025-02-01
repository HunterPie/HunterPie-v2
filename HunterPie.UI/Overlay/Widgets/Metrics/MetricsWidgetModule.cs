using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Metrics;

internal class MetricsWidgetModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<TelemetricsViewModel>();
    }
}