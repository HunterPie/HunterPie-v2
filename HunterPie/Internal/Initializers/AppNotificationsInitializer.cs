using HunterPie.Core.Notification;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Notification;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class AppNotificationsInitializer : IInitializer
{
    public Task Init()
    {
        var notificationService = new InAppNotificationService();
        _ = new NotificationService(notificationService);

        return Task.CompletedTask;
    }
}