using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

public record PoogieQuestDetailsModel(
    [property: JsonProperty("id")] int Id,
    [property: JsonProperty("type")] QuestType Type,
    [property: JsonProperty("deaths")] int Deaths,
    [property: JsonProperty("max_deaths")] int MaxDeaths,
    [property: JsonProperty("level")] QuestLevel Level,
    [property: JsonProperty("stars")] int Stars
)
{
    public QuestDetailsModel ToEntity() =>
        new QuestDetailsModel(
            Id: Id,
            Type: Type,
            Deaths: Deaths,
            MaxDeaths: MaxDeaths,
            Level: Level,
            Stars: Stars
        );

    public static PoogieQuestDetailsModel From(QuestDetailsModel model) =>
        new PoogieQuestDetailsModel(
            Id: model.Id,
            Type: model.Type,
            Deaths: model.Deaths,
            MaxDeaths: model.MaxDeaths,
            Level: model.Level,
            Stars: model.Stars
        );
}