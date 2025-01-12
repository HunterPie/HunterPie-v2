using System.Windows;
using System.Windows.Controls;
using SupportedGameViewModel = HunterPie.UI.Home.ViewModels.SupportedGameViewModel;

namespace HunterPie.UI.Home.Views;
/// <summary>
/// Interaction logic for SupportedGameView.xaml
/// </summary>
public partial class SupportedGameView : UserControl
{
    public SupportedGameView()
    {
        InitializeComponent();
    }

    private void OnSettingsClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SupportedGameViewModel viewModel)
            return;

        e.Handled = true;

        viewModel.OnSettings?.DynamicInvoke();
    }

    private void OnRunGameClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SupportedGameViewModel viewModel)
            return;

        e.Handled = true;
        viewModel.Execute?.DynamicInvoke();
    }
}