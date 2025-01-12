using HunterPie.UI.Settings.ViewModels.Internal;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Settings.Views;
/// <summary>
/// Interaction logic for MonsterDetailsConfigurationPropertyView.xaml
/// </summary>
public partial class MonsterDetailsConfigurationPropertyView : UserControl
{
    public MonsterDetailsConfigurationPropertyView()
    {
        InitializeComponent();
    }

    private void OnPartsConfigurationClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterDetailsPropertyViewModel vm)
            return;

        vm.ConfigureParts();
    }
}