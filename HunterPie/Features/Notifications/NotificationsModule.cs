using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Notifications.ViewModels;

namespace HunterPie.Features.Notifications;

internal class NotificationsModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<NotificationsPanelViewModel>();
    }
}