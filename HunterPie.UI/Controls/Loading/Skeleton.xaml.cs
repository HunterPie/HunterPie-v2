using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Loading;
/// <summary>
/// Interaction logic for Skeleton.xaml
/// </summary>
public partial class Skeleton : UserControl
{
    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static new readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(Skeleton));

    public Skeleton()
    {
        InitializeComponent();
    }
}