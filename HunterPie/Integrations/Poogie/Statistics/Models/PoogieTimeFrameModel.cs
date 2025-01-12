using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;
using System;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieTimeFrameModel(
    [property: JsonProperty("started_at")] DateTime StartedAt,
    [property: JsonProperty("finished_at")] DateTime FinishedAt
)
{
    public TimeFrameModel ToEntity() =>
        new TimeFrameModel(
            StartedAt: StartedAt,
            FinishedAt: FinishedAt
        );

    public static PoogieTimeFrameModel From(TimeFrameModel model) =>
        new PoogieTimeFrameModel(
            StartedAt: model.StartedAt,
            FinishedAt: model.FinishedAt
        );
}