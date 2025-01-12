using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Controls.Viewer;
public class SmoothScrollViewer : ScrollViewer
{
    private double _totalOffset;
    private readonly Queue<double> _offsetQueue = new();
    private bool _isScrolling;

    public double CurrentOffset
    {
        get => (double)GetValue(CurrentOffsetProperty);
        set => SetValue(CurrentOffsetProperty, value);
    }
    public static readonly DependencyProperty CurrentOffsetProperty =
        DependencyProperty.Register(nameof(CurrentOffset), typeof(double), typeof(ScrollViewer), new PropertyMetadata(0.0, OnCurrentOffsetChanged));

    public bool IsHorizontal
    {
        get => (bool)GetValue(IsHorizontalProperty);
        set => SetValue(IsHorizontalProperty, value);
    }
    public static readonly DependencyProperty IsHorizontalProperty =
        DependencyProperty.Register(nameof(IsHorizontal), typeof(bool), typeof(ScrollViewer), new PropertyMetadata(false));

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        double scrollableSize = IsHorizontal switch
        {
            true => ScrollableWidth,
            _ => ScrollableHeight,
        };

        if (scrollableSize == 0.0)
            return;

        e.Handled = true;

        double offset = IsHorizontal switch
        {
            true => HorizontalOffset,
            _ => VerticalOffset,
        };


        if (!_isScrolling)
        {
            _totalOffset = offset;
            CurrentOffset = offset;
        }

        double y = _totalOffset - (e.Delta / 2.0);
        _totalOffset = Math.Min(Math.Max(0, y), scrollableSize);
        AnimateScrolling(_totalOffset);
    }

    private static void OnCurrentOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not SmoothScrollViewer sv || e.NewValue is not double value)
            return;

        if (sv.IsHorizontal)
            sv.ScrollToHorizontalOffset(value);
        else
            sv.ScrollToVerticalOffset(value);
    }

    private void AnimateScrolling(double offset, double duration = 200)
    {
        _offsetQueue.Enqueue(offset);
        var animation = new DoubleAnimation(offset, TimeSpan.FromMilliseconds(duration))
        {
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            FillBehavior = FillBehavior.Stop
        };
        animation.Completed += OnAnimationCompleted;
        _isScrolling = true;

        BeginAnimation(CurrentOffsetProperty, animation, HandoffBehavior.Compose);
    }

    private void OnAnimationCompleted(object sender, EventArgs e)
    {
        if (sender is Timeline tl)
            tl.Completed -= OnAnimationCompleted;

        CurrentOffset = _offsetQueue.Dequeue();
        _isScrolling = false;
    }
}