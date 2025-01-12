using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record ChangePasswordRequest(
    [property: JsonProperty("email")] string Email,
    [property: JsonProperty("code")] string Code,
    [property: JsonProperty("new_password")] string NewPassword
);