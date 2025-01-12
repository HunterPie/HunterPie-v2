using Newtonsoft.Json;
using System;

namespace HunterPie.Integrations.Poogie.Notification.Models;

internal record NotificationResponse(
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("message")] string Message,
    [property: JsonProperty("icon")] string Icon,
    [property: JsonProperty("notification_type")] NotificationType NotificationType,
    [property: JsonProperty("primary_action")] string? PrimaryAction,
    [property: JsonProperty("secondary_action")] string? SecondaryAction,
    [property: JsonProperty("created_at")] DateTime CreatedAt
);