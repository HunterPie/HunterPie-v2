using System;

namespace HunterPie.Features.Statistics.Services;

internal interface IHuntStatisticsService<out T> : IDisposable
{
    public T Export();
}