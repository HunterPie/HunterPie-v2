using HunterPie.UI.Architecture;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for NativeButton.xaml
/// </summary>
public partial class Button : ClickableControl
{
    private readonly Storyboard _rippleAnimation;

    public new object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    public static new readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(object), typeof(Button));

    public new Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }
    public static new readonly DependencyProperty ForegroundProperty =
        DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.WhiteSmoke));

    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
    public static new readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(Button));

    public new VerticalAlignment VerticalContentAlignment
    {
        get => (VerticalAlignment)GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    public static new readonly DependencyProperty VerticalContentAlignmentProperty =
        DependencyProperty.Register(nameof(VerticalContentAlignment), typeof(VerticalAlignment), typeof(Button), new PropertyMetadata(VerticalAlignment.Center));

    public new HorizontalAlignment HorizontalContentAlignment
    {
        get => (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    // Using a DependencyProperty as the backing store for HorizontalContentAlignment.  This enables animation, styling, binding, etc...
    public static new readonly DependencyProperty HorizontalContentAlignmentProperty =
        DependencyProperty.Register(nameof(HorizontalContentAlignment), typeof(HorizontalAlignment), typeof(Button), new PropertyMetadata(HorizontalAlignment.Center));

    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    // Using a DependencyProperty as the backing store for Padding.  This enables animation, styling, binding, etc...
    public static new readonly DependencyProperty PaddingProperty =
        DependencyProperty.Register(nameof(Padding), typeof(Thickness), typeof(Button), new PropertyMetadata(new Thickness(2, 5, 2, 5)));

    public new Thickness BorderThickness
    {
        get => (Thickness)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    // Using a DependencyProperty as the backing store for BorderThickness.  This enables animation, styling, binding, etc...
    public static new readonly DependencyProperty BorderThicknessProperty =
        DependencyProperty.Register(nameof(BorderThickness), typeof(Thickness), typeof(Button));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Button), new PropertyMetadata(new CornerRadius(0)));

    public object? Key
    {
        get => (object?)GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register(nameof(Key), typeof(object), typeof(Button), new PropertyMetadata(null));

    public Button()
    {
        InitializeComponent();

        _rippleAnimation = FindResource("PART_RippleAnimation") as Storyboard;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        double targetWidth = Math.Max(ActualWidth, ActualHeight) * 2;
        Point mousePosition = e.GetPosition(this);
        var startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
        PART_Ripple.Margin = startMargin;
        (_rippleAnimation.Children[0] as DoubleAnimation).To = targetWidth;
        (_rippleAnimation.Children[1] as ThicknessAnimation).From = startMargin;
        (_rippleAnimation.Children[1] as ThicknessAnimation).To = new Thickness(mousePosition.X - (targetWidth / 2), mousePosition.Y - (targetWidth / 2), 0, 0);
        PART_Ripple.BeginStoryboard(_rippleAnimation);

        e.Handled = true;
    }
}