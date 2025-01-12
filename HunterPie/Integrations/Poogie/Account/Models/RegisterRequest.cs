using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record RegisterRequest(
    [property: JsonProperty("email")] string Email,
    [property: JsonProperty("username")] string Username,
    [property: JsonProperty("password")] string Password
);