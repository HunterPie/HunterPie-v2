using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Backup.Models;
internal record UserBackupDetailsResponse(
    [property: JsonProperty("count")] int Count,
    [property: JsonProperty("max_count")] int MaxCount,
    [property: JsonProperty("backups")] BackupResponse[] Backups
);