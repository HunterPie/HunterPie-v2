using Newtonsoft.Json;
using System.Collections.Generic;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record MyUserAccountResponse(
    [property: JsonProperty("username")] string Username,
    [property: JsonProperty("email")] string Email,
    [property: JsonProperty("avatar_url")] string AvatarUrl,
    [property: JsonProperty("experience")] int Experience,
    [property: JsonProperty("rating")] int Rating,
    [property: JsonProperty("badges")] List<object> Badges,
    [property: JsonProperty("is_supporter")] bool IsSupporter,
    [property: JsonProperty("tier")] SupporterTier SupporterTier
);