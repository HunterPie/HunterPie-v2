using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record LogoutResponse([property: JsonProperty("message")] string Message);