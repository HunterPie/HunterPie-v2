using HunterPie.Core.Client;
using HunterPie.Core.Domain.Dialog;
using HunterPie.UI.Controls.Notification;
using HunterPie.UI.Controls.Notification.ViewModels;
using HunterPie.UI.Main.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.UI.Main.Views;
/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private void OnDragStart(object sender, RoutedEventArgs e) => DragMove();

    private void OnCloseClick(object sender, RoutedEventArgs e) => Close();

    private void OnMinimizeClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;

        if (ClientConfig.Config.Client.MinimizeToSystemTray)
            Hide();
    }

    private void OnNotificationClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Toast { DataContext: ToastViewModel vm })
            return;

        vm.IsVisible = false;
    }

    protected override async void OnClosing(CancelEventArgs e)
    {
        if (DataContext is not MainViewModel vm)
            return;

        if (!ClientConfig.Config.Client.EnableSeamlessShutdown)
        {
            NativeDialogResult result = DialogManager.Info(
                title: Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='CONFIRMATION_TITLE_STRING']"),
                description: Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='EXIT_CONFIRMATION_DESCRIPTION_STRING']"),
                buttons: NativeDialogButtons.Accept | NativeDialogButtons.Cancel
            );

            if (result != NativeDialogResult.Accept)
            {
                e.Cancel = true;
                return;
            }
        }

        await Dispatcher.InvokeAsync(Hide);
        await vm.GracefulShutdown();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && e.KeyboardDevice.IsKeyDown(Key.R))
            App.Restart();
    }
}