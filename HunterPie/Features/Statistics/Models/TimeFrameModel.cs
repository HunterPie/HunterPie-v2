using System;

namespace HunterPie.Features.Statistics.Models;

internal record TimeFrameModel(
    DateTime StartedAt,
    DateTime FinishedAt
)
{
    public static TimeFrameModel Start() => new TimeFrameModel(
        StartedAt: DateTime.UtcNow,
        FinishedAt: DateTime.MaxValue
    );

    public TimeFrameModel End() => IsRunning() ? this with { FinishedAt = DateTime.UtcNow } : this;

    public bool IsRunning() => FinishedAt == DateTime.MaxValue;
}