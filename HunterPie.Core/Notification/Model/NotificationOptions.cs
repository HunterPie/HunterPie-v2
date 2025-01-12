using System;

namespace HunterPie.Core.Notification.Model;

#nullable enable
public record NotificationOptions(
    NotificationType Type,
    string Title,
    string Description,
    TimeSpan? DisplayTime = null,
    NotificationCallback? PrimaryCallback = null,
    NotificationCallback? SecondaryCallback = null
);