using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Supporter.Models;

public record SupporterValidationResponse(
    [property: JsonProperty("is_valid")] bool IsValid
);