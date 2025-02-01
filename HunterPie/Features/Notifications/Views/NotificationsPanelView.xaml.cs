using HunterPie.DI;
using HunterPie.Features.Notifications.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Notifications.Views;

/// <summary>
/// Interaction logic for NotificationsPanelView.xaml
/// </summary>
public partial class NotificationsPanelView : UserControl
{
    private NotificationsPanelViewModel ViewModel => (NotificationsPanelViewModel)DataContext;

    public NotificationsPanelView()
    {
        DataContext = DependencyContainer.Get<NotificationsPanelViewModel>();
        InitializeComponent();
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        await ViewModel.FetchNotificationsAsync();
    }
}