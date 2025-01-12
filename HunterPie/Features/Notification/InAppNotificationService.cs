using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.UI.Architecture.Dispatchers;
using HunterPie.UI.Controls.Notification.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.Features.Notification;

internal class InAppNotificationService : INotificationService
{
    private static readonly Dictionary<Guid, (DispatcherJob, ToastViewModel)> ViewModelsLookup = new();
    public static ObservableCollection<ToastViewModel> Notifications { get; } = new();

    public async Task<Guid> Show(NotificationOptions options)
    {
        var notificationId = Guid.NewGuid();
        var notification = new ToastViewModel
        {
            Type = options.Type,
            Title = options.Title,
            Description = options.Description,
            IsVisible = true,
            PrimaryLabel = options.PrimaryCallback?.Label,
            PrimaryHandler = options.PrimaryCallback?.Handler,
            SecondaryLabel = options.SecondaryCallback?.Label,
            SecondaryHandler = options.SecondaryCallback?.Handler,
        };

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            lock (Notifications)
            {
                var job = new DispatcherJob(() => HandleVisibilityTimeout(notificationId), options.DisplayTime);

                ViewModelsLookup.Add(notificationId, (job, notification));
                Notifications.Add(notification);
            }
        });

        return notificationId;
    }

    public void Update(Guid id, NotificationOptions options)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            lock (Notifications)
            {
                if (!ViewModelsLookup.TryGetValue(id, out (DispatcherJob, ToastViewModel vm) value))
                    return;

                value.vm.Type = options.Type;
                value.vm.Title = options.Title;
                value.vm.Description = options.Description;
                value.vm.PrimaryLabel = options.PrimaryCallback?.Label;
                value.vm.PrimaryHandler = options.PrimaryCallback?.Handler;
                value.vm.SecondaryLabel = options.SecondaryCallback?.Label;
                value.vm.SecondaryHandler = options.SecondaryCallback?.Handler;
            }
        });
    }

    private static void HandleVisibilityTimeout(Guid notificationId)
    {
        lock (Notifications)
        {
            if (!ViewModelsLookup.TryGetValue(notificationId, out (DispatcherJob, ToastViewModel vm) value))
                return;

            ViewModelsLookup.Remove(notificationId);
            Notifications.Remove(value.vm);
        }
    }
}