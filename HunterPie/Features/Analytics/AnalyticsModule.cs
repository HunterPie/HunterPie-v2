using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Analytics.Services;
using HunterPie.Features.Analytics.Strategies;

namespace HunterPie.Features.Analytics;

internal class AnalyticsModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<CrashEventStrategy>()
            .WithService<AnalyticsService>();
    }
}