using HunterPie.Integrations.Poogie.Common.Models;
using Newtonsoft.Json;
using System;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieQuestSummaryModel(
    [property: JsonProperty("id")] string Id,
    [property: JsonProperty("game_type")] GameType GameType,
    [property: JsonProperty("quest")] PoogieQuestDetailsModel? QuestDetails,
    [property: JsonProperty("elapsed_time")] TimeSpan? ElapsedTime,
    [property: JsonProperty("monsters")] PoogieMonsterSummaryModel[] Monsters,
    [property: JsonProperty("created_at")] DateTime CreatedAt
);