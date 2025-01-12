using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Settings.Models;

internal record ClientSettingsRequest(
    [property: JsonProperty("configuration")] string Configuration
);