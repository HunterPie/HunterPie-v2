using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record RegisterResponse(
    [property: JsonProperty("username")] string Username,
    [property: JsonProperty("email")] string Email,
    [property: JsonProperty("avatar_url")] string AvatarUrl,
    [property: JsonProperty("experience")] int Experience,
    [property: JsonProperty("rating")] int Rating,
    [property: JsonProperty("is_supporter")] bool IsSupporter
);