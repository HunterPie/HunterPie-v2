using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record LoginResponse([property: JsonProperty("token")] string Token);