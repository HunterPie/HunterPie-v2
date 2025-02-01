using HunterPie.Core.Notification.Model;
using System;
using System.Threading.Tasks;

namespace HunterPie.Core.Notification;
public interface INotificationService
{
    public Task<Guid> Show(NotificationOptions options);

    public void Update(Guid id, NotificationOptions options);
}