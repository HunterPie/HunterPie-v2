using HunterPie.UI.Controls.Notification.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Notification;
/// <summary>
/// Interaction logic for Toast.xaml
/// </summary>
public partial class Toast : UserControl
{
    public Toast()
    {
        InitializeComponent();
    }

    private void OnPrimaryButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ToastViewModel vm)
            return;

        vm.PrimaryHandler?.Invoke(sender, EventArgs.Empty);
    }

    private void OnSecondaryButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ToastViewModel vm)
            return;

        vm.SecondaryHandler?.Invoke(sender, EventArgs.Empty);
    }
}