using HunterPie.Core.Architecture;
using Newtonsoft.Json;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MonsterAilmentConfiguration
{
    public required int Id { get; init; }

    [JsonIgnore]
    public required string StringId { get; init; }

    public Observable<bool> IsEnabled { get; set; } = true;
}