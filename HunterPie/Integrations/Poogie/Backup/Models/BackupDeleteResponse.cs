using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Backup.Models;

internal record BackupDeleteResponse([property: JsonProperty("id")] string Id);