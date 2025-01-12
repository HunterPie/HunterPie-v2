using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record PasswordChangeResponse([property: JsonProperty("success")] bool Success);