using HunterPie.Core.Analytics.Entity;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Analytics.Strategies;

internal interface IAnalyticsStrategy
{
    bool CanHandle(Type type);
    Task SendAsync(IAnalyticsEvent analyticsEvent);
}