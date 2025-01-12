using HunterPie.Core.Converters;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MonsterConfiguration
{
    public required int Id { get; init; }

    [JsonConverter(typeof(ObservableCollectionConverter<MonsterPartConfiguration>))]
    public required ObservableCollection<MonsterPartConfiguration> Parts { get; set; } = new();

    [JsonConverter(typeof(ObservableCollectionConverter<MonsterAilmentConfiguration>))]
    public required ObservableCollection<MonsterAilmentConfiguration> Ailments { get; set; } = new();

    public override int GetHashCode() => Id;

    public override bool Equals(object? obj)
    {
        if (obj is not MonsterConfiguration configuration)
            return false;

        return Id == configuration.Id;
    }
}