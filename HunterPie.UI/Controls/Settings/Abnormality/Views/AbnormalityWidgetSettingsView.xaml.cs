using HunterPie.UI.Controls.Settings.Abnormality.ViewModels;
using HunterPie.UI.Controls.TextBox.Events;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Abnormality.Views;
/// <summary>
/// Interaction logic for AbnormalityWidgetSettingsView.xaml
/// </summary>
public partial class AbnormalityWidgetSettingsView : UserControl
{
    public AbnormalityWidgetSettingsView()
    {
        InitializeComponent();
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityWidgetSettingsViewModel vm)
            return;

        vm.ExitScreen();
    }

    private void OnAbnormalityClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityWidgetSettingsViewModel vm)
            return;

        if (sender is not AbnormalityElementView { DataContext: AbnormalityElementViewModel element })
            return;

        vm.ToggleAbnormality(element.Id);
    }

    private void OnSelectAllClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityWidgetSettingsViewModel vm)
            return;

        vm.SelectAllFromCurrentCategory();
    }

    private void OnUnselectAllClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityWidgetSettingsViewModel vm)
            return;

        vm.UnselectAllFromCurrentCategory();
    }

    private void OnSearchTextChange(object sender, SearchTextChangedEventArgs e)
    {
        if (DataContext is not AbnormalityWidgetSettingsViewModel vm)
            return;

        vm.Search(e.Text);
    }
}