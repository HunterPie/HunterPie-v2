using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieMonsterSummaryModel(
    [property: JsonProperty("id")] int Id,
    [property: JsonProperty("is_target")] bool IsTarget,
    [property: JsonProperty("hunt_type")] MonsterHuntType? HuntType
);