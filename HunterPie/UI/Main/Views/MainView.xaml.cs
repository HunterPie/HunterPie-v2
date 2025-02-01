using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;
using HunterPie.Core.Domain.Dialog;
using HunterPie.UI.Controls.Notification;
using HunterPie.UI.Controls.Notification.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace HunterPie.UI.Main.Views;
/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Window
{
    private readonly ILocalizationRepository _localizationRepository;

    public MainView(ILocalizationRepository localizationRepository)
    {
        _localizationRepository = localizationRepository;
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
        if (!ClientConfig.Config.Client.EnableSeamlessShutdown)
        {
            LocalizationData confirmationTitle = _localizationRepository.FindBy("//Strings/Client/Dialogs/Dialog[@Id='CONFIRMATION_TITLE_STRING']");
            LocalizationData exitDescription = _localizationRepository.FindBy("//Strings/Client/Dialogs/Dialog[@Id='EXIT_CONFIRMATION_DESCRIPTION_STRING']");

            NativeDialogResult result = DialogManager.Info(
                title: confirmationTitle.String,
                description: exitDescription.String,
                buttons: NativeDialogButtons.Accept | NativeDialogButtons.Cancel
            );

            if (result != NativeDialogResult.Accept)
            {
                e.Cancel = true;
                return;
            }
        }

        await Dispatcher.InvokeAsync(Hide);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && e.KeyboardDevice.IsKeyDown(Key.R))
            App.Restart();
    }
}