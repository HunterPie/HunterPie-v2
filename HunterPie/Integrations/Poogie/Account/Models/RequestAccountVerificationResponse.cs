using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

public record RequestAccountVerificationResponse(
    [property: JsonProperty("ok")] bool Ok
);