using HunterPie.Core.Notification.Model;
using System;
using System.Threading.Tasks;

namespace HunterPie.Core.Notification;

#nullable enable
public class NotificationService
{
    private readonly INotificationService _service;
    private static NotificationService? _instance;

    internal NotificationService(INotificationService service)
    {
        _service = service;
        _instance = this;
    }

    public static async Task<Guid> Show(NotificationOptions options) => await _instance!._service.Show(options);

    public static void Update(Guid id, NotificationOptions options) => _instance!._service.Update(id, options);
}