using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record RequestAccountVerifyRequest(
    [property: JsonProperty("email")] string Email
);