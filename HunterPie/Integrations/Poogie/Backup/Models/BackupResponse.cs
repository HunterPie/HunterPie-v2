using Newtonsoft.Json;
using System;

namespace HunterPie.Integrations.Poogie.Backup.Models;

internal record BackupResponse(
    [property: JsonProperty("id")] string Id,
    [property: JsonProperty("size")] long Size,
    [property: JsonProperty("game_name")] string GameName,
    [property: JsonProperty("game_icon")] string GameIcon,
    [property: JsonProperty("uploaded_at")] DateTime UploadedAt
);