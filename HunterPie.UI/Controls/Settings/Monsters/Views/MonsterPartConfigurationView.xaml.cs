using HunterPie.Core.Client.Configuration.Overlay.Monster;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Monsters.Views;
/// <summary>
/// Interaction logic for MonsterPartConfigurationViewModel.xaml
/// </summary>
public partial class MonsterPartConfigurationView : UserControl
{
    public MonsterPartConfigurationView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterPartConfiguration vm)
            return;

        vm.IsEnabled.Value = !vm.IsEnabled;
    }
}