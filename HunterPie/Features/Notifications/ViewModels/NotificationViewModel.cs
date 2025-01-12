using HunterPie.Integrations.Poogie.Notification.Models;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.Features.Notifications.ViewModels;

internal class NotificationViewModel : ViewModel
{
    private string _title;
    private string _message;
    private NotificationType _type;
    private DateTime _date;

    public string Title { get => _title; set => SetValue(ref _title, value); }
    public string Message { get => _message; set => SetValue(ref _message, value); }
    public NotificationType Type { get => _type; set => SetValue(ref _type, value); }
    public DateTime Date { get => _date; set => SetValue(ref _date, value); }
}