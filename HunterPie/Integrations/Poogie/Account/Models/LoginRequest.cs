using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record LoginRequest(
    [property: JsonProperty("email")] string Email,
    [property: JsonProperty("password")] string Password
);