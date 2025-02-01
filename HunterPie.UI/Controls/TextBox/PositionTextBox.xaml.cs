using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.TextBox;

/// <summary>
/// Interaction logic for PositionTextBox.xaml
/// </summary>
public partial class PositionTextBox : UserControl
{
    public double X
    {
        get => (double)GetValue(XProperty);
        set => SetValue(XProperty, value);
    }
    public static readonly DependencyProperty XProperty =
        DependencyProperty.Register(nameof(X), typeof(double), typeof(PositionTextBox), new PropertyMetadata(0.0));

    public double Y
    {
        get => (double)GetValue(YProperty);
        set => SetValue(YProperty, value);
    }
    public static readonly DependencyProperty YProperty =
        DependencyProperty.Register(nameof(Y), typeof(double), typeof(PositionTextBox), new PropertyMetadata(0.0));

    public PositionTextBox()
    {
        InitializeComponent();
    }
}