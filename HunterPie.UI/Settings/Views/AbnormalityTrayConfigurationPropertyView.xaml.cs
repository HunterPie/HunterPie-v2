using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Dialog;
using HunterPie.UI.Settings.ViewModels.Internal;
using System.Windows;
using System.Windows.Controls;
using Button = HunterPie.UI.Controls.Buttons.Button;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.UI.Settings.Views;
/// <summary>
/// Interaction logic for AbnormalityTrayConfigurationPropertyView.xaml
/// </summary>
public partial class AbnormalityTrayConfigurationPropertyView : UserControl
{
    public AbnormalityTrayConfigurationPropertyView()
    {
        InitializeComponent();
    }

    private void OnCreateNewTrayButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityTrayPropertyViewModel vm)
            return;

        vm.CreateNewTray();
    }

    private void OnDeleteTrayButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityTrayPropertyViewModel vm)
            return;

        if (sender is not Button { Key: AbnormalityWidgetConfig tray })
            return;

        NativeDialogResult dialogResult = DialogManager.Warn(
            Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='CONFIRMATION_TITLE_STRING']"),
            Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='DELETE_CONFIRMATION_DESCRIPTION_STRING']")
                    .Replace("{Item}", $"{(string)tray.Name}"),
            NativeDialogButtons.Accept | NativeDialogButtons.Cancel
        );

        if (dialogResult != NativeDialogResult.Accept)
            return;

        vm.DeleteTray(tray);
    }

    private void OnConfigureTrayButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityTrayPropertyViewModel vm)
            return;

        if (sender is not Button { Key: AbnormalityWidgetConfig tray })
            return;

        vm.ConfigureTray(tray);
    }
}