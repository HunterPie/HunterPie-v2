using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Controls.Progress;

/// <summary>
/// Interaction logic for Bar.xaml
/// </summary>
public class Bar : UserControl
{

    /// <summary>
    /// Controls the current value of the bar
    /// </summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(Bar), new PropertyMetadata(0.0));

    /// <summary>
    /// Controls the maximum value of the bar
    /// </summary>
    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }
    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(Bar), new PropertyMetadata(0.0));

    /// <summary>
    /// Controls the border thickness
    /// </summary>
    public Thickness Border
    {
        get => (Thickness)GetValue(BorderProperty);
        set => SetValue(BorderProperty, value);
    }
    public static readonly DependencyProperty BorderProperty =
        DependencyProperty.Register(nameof(Border), typeof(Thickness), typeof(Bar));

    /// <summary>
    /// Controls the color of the background border
    /// </summary>
    public Brush BackgroundBorder
    {
        get => (Brush)GetValue(BackgroundBorderProperty);
        set => SetValue(BackgroundBorderProperty, value);
    }
    public static readonly DependencyProperty BackgroundBorderProperty =
        DependencyProperty.Register(nameof(BackgroundBorder), typeof(Brush), typeof(Bar));

    /// <summary>
    /// Controls the color of the background bar
    /// </summary>
    public Brush BackgroundColor
    {
        get => (Brush)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }
    public static readonly DependencyProperty BackgroundColorProperty =
        DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(Bar));

    /// <summary>
    /// Controls the color of the foreground border
    /// </summary>
    public Brush ForegroundBorder
    {
        get => (Brush)GetValue(ForegroundBorderProperty);
        set => SetValue(ForegroundBorderProperty, value);
    }
    public static readonly DependencyProperty ForegroundBorderProperty =
        DependencyProperty.Register(nameof(ForegroundBorder), typeof(Brush), typeof(Bar));

    /// <summary>
    /// Controls the color of the foreground color
    /// </summary>
    public Brush ForegroundColor
    {
        get => (Brush)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }
    public static readonly DependencyProperty ForegroundColorProperty =
        DependencyProperty.Register(nameof(ForegroundColor), typeof(Brush), typeof(Bar));

    /// <summary>
    /// Sets the content inside the bar
    /// </summary>
    public new FrameworkElement Content
    {
        get => (FrameworkElement)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    public static new readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(FrameworkElement), typeof(Bar));
}