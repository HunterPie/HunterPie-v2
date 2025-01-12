using HunterPie.UI.SideBar.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.SideBar.Views;

/// <summary>
/// Interaction logic for SideBarButtonView.xaml
/// </summary>
public partial class SideBarButtonView : UserControl
{
    public SideBarButtonView()
    {
        InitializeComponent();
    }

    private async void OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ISideBarViewModel vm)
            return;

        await vm.ExecuteAsync();
    }
}