using HunterPie.Core.Client.Configuration.Overlay.Monster;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Monsters.Views;
/// <summary>
/// Interaction logic for MonsterAilmentConfigurationView.xaml
/// </summary>
public partial class MonsterAilmentConfigurationView : UserControl
{
    public MonsterAilmentConfigurationView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterAilmentConfiguration vm)
            return;

        vm.IsEnabled.Value = !vm.IsEnabled;
    }
}