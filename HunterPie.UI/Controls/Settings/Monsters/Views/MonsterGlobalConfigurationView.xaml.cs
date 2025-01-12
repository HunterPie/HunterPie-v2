using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Monsters.Views;

/// <summary>
/// Interaction logic for MonsterGlobalConfigurationView.xaml
/// </summary>
public partial class MonsterGlobalConfigurationView : UserControl
{
    public MonsterGlobalConfigurationView()
    {
        InitializeComponent();
    }

    private void OnPartGroupClick(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: MonsterPartGroupViewModel vm })
            return;

        vm.Toggle();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterGlobalConfigurationViewModel vm)
            return;

        vm.IsExpanded = !vm.IsExpanded;
    }

    private void OnAilmentClick(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: MonsterGlobalAilmentViewModel vm })
            return;

        vm.Toggle();
    }
}