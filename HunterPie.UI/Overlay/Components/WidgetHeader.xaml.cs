using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Overlay.Components;

/// <summary>
/// Interaction logic for WidgetHeader.xaml
/// </summary>
public partial class WidgetHeader : UserControl
{
    public WidgetBase Owner { get; private set; }

    public WidgetHeader()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        if (Owner.Widget.Settings is null)
            return;

        Owner.Widget.Settings.Initialize.Value = false;
    }

    private void OnHideButtonClick(object sender, RoutedEventArgs e)
    {
        if (Owner.Widget.Settings is null)
            return;

        Owner.Widget.Settings.Enabled.Value = !Owner.Widget.Settings.Enabled.Value;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => Owner = (WidgetBase)Window.GetWindow(this);

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        Owner.DragMove();
    }
}