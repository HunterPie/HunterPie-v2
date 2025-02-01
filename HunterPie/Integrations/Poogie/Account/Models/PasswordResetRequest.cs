using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;
internal record PasswordResetRequest([property: JsonProperty("email")] string Email);