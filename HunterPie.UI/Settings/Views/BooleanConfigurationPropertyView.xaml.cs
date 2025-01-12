using HunterPie.UI.Settings.ViewModels.Internal;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Settings.Views;

/// <summary>
/// Interaction logic for BooleanConfigurationPropertyView.xaml
/// </summary>
public partial class BooleanConfigurationPropertyView : UserControl
{
    public BooleanConfigurationPropertyView()
    {
        InitializeComponent();
    }

    private void OnLeftMouseUp(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;

        if (DataContext is not BooleanPropertyViewModel vm)
            return;

        vm.Boolean.Value = !vm.Boolean.Value;
    }
}