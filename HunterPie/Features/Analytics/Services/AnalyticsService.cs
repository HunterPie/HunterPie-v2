using HunterPie.Core.Analytics;
using HunterPie.Core.Analytics.Entity;
using HunterPie.Features.Analytics.Strategies;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Analytics.Services;

internal class AnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsStrategy[] _strategies;

    public AnalyticsService(
        IAnalyticsStrategy[] strategies
    )
    {
        _strategies = strategies;
    }

    public async Task SendAsync(IAnalyticsEvent analyticsEvent)
    {
        IAnalyticsStrategy? strategy =
            _strategies.FirstOrDefault(it => it.CanHandle(analyticsEvent.GetType()));

        if (strategy is null)
            return;

        await strategy.SendAsync(analyticsEvent);
    }
}