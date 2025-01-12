using HunterPie.Core.Game.Entity.Game.Quest;

namespace HunterPie.Features.Statistics.Models;

public record QuestDetailsModel(
    int Id,
    QuestType Type,
    int Deaths,
    int MaxDeaths,
    QuestLevel Level,
    int Stars
);