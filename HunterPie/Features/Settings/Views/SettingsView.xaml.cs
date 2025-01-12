using HunterPie.Features.Settings.ViewModels;
using HunterPie.UI.Controls.TextBox.Events;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Settings.Views;
/// <summary>
/// Interaction logic for SettingsView.xaml
/// </summary>
public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void OnSearchTextChanged(object? sender, SearchTextChangedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.Search(e.Text);
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.ChangeSettingsGroup();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.FetchVersion();
    }

    private void OnRetryVersionFetchClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.FetchVersion();
    }

    private void OnDownloadVersionClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.ExecuteUpdate();
    }
}