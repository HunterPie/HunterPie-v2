using Newtonsoft.Json;
using System.Collections.Generic;

namespace HunterPie.Integrations.Poogie.Localization.Models;

public record LocalizationResponse(
    [property: JsonProperty("localizations")] Dictionary<string, string> Localizations
);