using HunterPie.UI.Home.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Home.Views;

/// <summary>
/// Interaction logic for HomeCallToActionView.xaml
/// </summary>
public partial class HomeCallToActionView : UserControl
{
    public HomeCallToActionView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not HomeCallToActionViewModel vm)
            return;

        vm.Execute();
    }
}