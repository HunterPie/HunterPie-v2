using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Misc;

/// <summary>
/// Interaction logic for Badge.xaml
/// </summary>
public partial class Badge : UserControl
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(Badge), new PropertyMetadata("New"));

    public new Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
    public static new readonly DependencyProperty ForegroundProperty =
        DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(Badge), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xA8, 0xA8))));

    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
    public static new readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(Badge), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xCC, 0x70, 0x00, 0x00))));

    public Badge()
    {
        InitializeComponent();
    }
}