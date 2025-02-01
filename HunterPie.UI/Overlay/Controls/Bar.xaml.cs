using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Overlay.Controls;

/// <summary>
/// Interaction logic for Bar.xaml
/// </summary>
public partial class Bar : UserControl
{

    private readonly DoubleAnimation _cachedAnimation = new DoubleAnimation() { EasingFunction = new QuadraticEase(), Duration = new Duration(TimeSpan.FromMilliseconds(200)) };

    public double ActualValueDelayed
    {
        get => (double)GetValue(ActualValueDelayedProperty);
        set => SetValue(ActualValueDelayedProperty, value);
    }
    public static readonly DependencyProperty ActualValueDelayedProperty =
        DependencyProperty.Register("ActualValueDelayed", typeof(double), typeof(Bar));

    public double ActualValue
    {
        get => (double)GetValue(ActualValueProperty);
        set => SetValue(ActualValueProperty, value);
    }
    public static readonly DependencyProperty ActualValueProperty =
        DependencyProperty.Register("ActualValue", typeof(double), typeof(Bar));

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(double), typeof(Bar), new PropertyMetadata(0.0, OnValueChange));

    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register("MaxValue", typeof(double), typeof(Bar));

    public Brush ForegroundDelayed
    {
        get => (Brush)GetValue(ForegroundDelayedProperty);
        set => SetValue(ForegroundDelayedProperty, value);
    }
    public static readonly DependencyProperty ForegroundDelayedProperty =
        DependencyProperty.Register("ForegroundDelayed", typeof(Brush), typeof(Bar));

    public Visibility MarkersVisibility
    {
        get => (Visibility)GetValue(MarkersVisibilityProperty);
        set => SetValue(MarkersVisibilityProperty, value);
    }
    public static readonly DependencyProperty MarkersVisibilityProperty =
        DependencyProperty.Register("MarkersVisibility", typeof(Visibility), typeof(Bar), new PropertyMetadata(Visibility.Visible));

    public Bar()
    {
        InitializeComponent();
    }

    private static void OnValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var owner = d as Bar;

        if (owner.MaxValue == 0.0)
            return;

        double newValue = (owner.ActualWidth * ((double)e.NewValue / owner.MaxValue)) - 4;

        newValue = Math.Max(1.0, newValue);

        if (double.IsNaN(newValue))
            return;

        if (double.IsInfinity(owner.ActualValue) || double.IsInfinity(newValue))
            return;

        DoubleAnimation animation = owner._cachedAnimation;
        animation.From = owner.ActualValue;
        animation.To = newValue;

        owner.BeginAnimation(Bar.ActualValueProperty, animation, HandoffBehavior.SnapshotAndReplace);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {

        double value = (e.NewSize.Width * (Value / MaxValue)) - (BorderThickness.Left + BorderThickness.Right);

        value = Math.Max(1.0, value);

        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            InvalidateVisual();
            return;
        }

        DoubleAnimation smoothAnimation = _cachedAnimation;
        smoothAnimation.From = value;
        smoothAnimation.To = value;

        BeginAnimation(Bar.ActualValueProperty, smoothAnimation, HandoffBehavior.SnapshotAndReplace);
    }
}