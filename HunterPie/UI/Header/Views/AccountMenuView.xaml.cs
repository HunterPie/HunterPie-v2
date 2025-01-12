using HunterPie.UI.Architecture.Utils;
using HunterPie.UI.Header.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HunterPie.UI.Header.Views;

/// <summary>
/// Interaction logic for AccountMenuView.xaml
/// </summary>
public partial class AccountMenuView : UserControl
{
    public AccountMenuView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AccountMenuViewModel vm)
            return;

        vm.IsOpen = !vm.IsOpen;
    }

    private void OnDropDownChanged(object? sender, DataTransferEventArgs e)
    {
        if (DataContext is not AccountMenuViewModel vm)
            return;

        if (vm.IsOpen)
            Mouse.Capture(this, CaptureMode.SubTree);
        else
            Mouse.Capture(null);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AccountMenuViewModel vm)
            return;

        var parentWindow = Window.GetWindow(this);

        if (parentWindow is null)
            return;

        parentWindow.Deactivated += (_, __) => vm.IsOpen = false;
    }

    private void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is not AccountMenuViewModel vm)
            return;

        if (Mouse.Captured is null)
            return;

        Point buttonDistance = e.GetPosition(this);
        bool isWithinBounds = buttonDistance.IsWithinBounds(this);

        if (isWithinBounds)
            return;

        vm.IsOpen = false;
    }

    private void OnSignInButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AccountMenuViewModel vm)
            return;

        vm.OpenSignInScreen();
    }

    private void OnSignOutClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AccountMenuViewModel vm)
            return;

        vm.SignOut();
    }

    private void OnAccountDetailsClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AccountMenuViewModel vm)
            return;

        vm.OpenAccountDetails();
    }

    private void OnAccountSettingsClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AccountMenuViewModel vm)
            return;

        vm.OpenAccountSettings();
    }
}