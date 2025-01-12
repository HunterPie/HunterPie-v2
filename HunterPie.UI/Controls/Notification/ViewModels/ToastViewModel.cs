using HunterPie.Core.Notification.Model;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.Controls.Notification.ViewModels;

#nullable enable
public class ToastViewModel : ViewModel
{
    private NotificationType _type;
    public NotificationType Type { get => _type; set => SetValue(ref _type, value); }

    private string _title = string.Empty;
    public string Title { get => _title; set => SetValue(ref _title, value); }

    private string _description = string.Empty;
    public string Description { get => _description; set => SetValue(ref _description, value); }

    private string? _primaryLabel;
    public string? PrimaryLabel { get => _primaryLabel; set => SetValue(ref _primaryLabel, value); }

    private EventHandler<EventArgs>? _primaryHandler;
    public EventHandler<EventArgs>? PrimaryHandler { get => _primaryHandler; set => SetValue(ref _primaryHandler, value); }

    private string? _secondaryLabel;
    public string? SecondaryLabel { get => _secondaryLabel; set => SetValue(ref _secondaryLabel, value); }

    private EventHandler<EventArgs>? _secondaryHandler;
    public EventHandler<EventArgs>? SecondaryHandler { get => _secondaryHandler; set => SetValue(ref _secondaryHandler, value); }

    private int _buttonCount;
    public int ButtonCount { get => _buttonCount; set => SetValue(ref _buttonCount, value); }

    private bool _isVisible = true;
    public bool IsVisible { get => _isVisible; set => SetValue(ref _isVisible, value); }
}