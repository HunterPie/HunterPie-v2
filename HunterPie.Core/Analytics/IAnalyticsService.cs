using HunterPie.Core.Analytics.Entity;
using System.Threading.Tasks;

namespace HunterPie.Core.Analytics;

public interface IAnalyticsService
{
    Task SendAsync(IAnalyticsEvent analyticsEvent);
}