using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;
using System.Linq;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieMonsterStatusStatisticsModel(
    [property: JsonProperty("activations")] PoogieTimeFrameModel[] Activations
)
{
    public MonsterStatusModel ToEntity() =>
        new MonsterStatusModel(
            Activations: Activations.Select(it => it.ToEntity()).ToArray()
        );

    public static PoogieMonsterStatusStatisticsModel From(MonsterStatusModel model) =>
        new PoogieMonsterStatusStatisticsModel(
            Activations: model.Activations.Select(PoogieTimeFrameModel.From).ToArray()
        );
}