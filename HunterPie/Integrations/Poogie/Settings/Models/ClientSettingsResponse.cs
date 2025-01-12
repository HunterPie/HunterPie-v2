using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Settings.Models;

internal record ClientSettingsResponse(
    [property: JsonProperty("configuration")] string Configuration
);