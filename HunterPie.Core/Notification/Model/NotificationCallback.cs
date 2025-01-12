using System;

namespace HunterPie.Core.Notification.Model;

public record NotificationCallback(
    string Label,
    EventHandler<EventArgs> Handler
);