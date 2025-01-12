using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Backup.Models;

internal record CanUploadBackupResponse([property: JsonProperty("can_upload")] bool CanUpload);