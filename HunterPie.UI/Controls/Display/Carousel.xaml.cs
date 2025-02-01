using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Controls.Display;
/// <summary>
/// Interaction logic for Carousel.xaml
/// </summary>
#nullable enable
public partial class Carousel : UserControl
{
    private static readonly Duration DefaultDuration = new Duration(TimeSpan.FromSeconds(5));
    private DoubleAnimation? _animation = null;

    public IList? ItemsSource
    {
        get => (IList?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IList), typeof(Carousel), new PropertyMetadata(OnItemsSourceChange));

    public DataTemplateSelector ItemTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
        set => SetValue(ItemTemplateSelectorProperty, value);
    }
    public static readonly DependencyProperty ItemTemplateSelectorProperty =
        DependencyProperty.Register(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(Carousel));

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }
    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(Carousel));

    public object Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }
    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register(nameof(Item), typeof(object), typeof(object));

    public int SelectedItem
    {
        get => (int)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(int), typeof(Carousel), new PropertyMetadata(OnSelectedItemChange));

    public double ProgressPercentage
    {
        get => (double)GetValue(ProgressPercentageProperty);
        set => SetValue(ProgressPercentageProperty, value);
    }
    public static readonly DependencyProperty ProgressPercentageProperty =
        DependencyProperty.Register(nameof(ProgressPercentage), typeof(double), typeof(Carousel), new PropertyMetadata(0.0));

    public Duration Duration
    {
        get => (Duration)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }
    public static readonly DependencyProperty DurationProperty =
        DependencyProperty.Register(nameof(Duration), typeof(Duration), typeof(Carousel), new PropertyMetadata(DefaultDuration, OnDurationChanged));

    public Carousel()
    {
        InitializeComponent();
    }

    private void OnPreviousClick(object sender, RoutedEventArgs e)
    {
        if (ItemsSource is null)
            return;

        int nextSlide = SelectedItem - 1;

        SelectedItem = nextSlide >= 0 ? nextSlide : ItemsSource.Count - 1;
        RestartAnimation();
    }

    private void OnNextClick(object sender, RoutedEventArgs e) => NextItem();

    private static void OnItemsSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Carousel carousel)
            return;

        if (carousel.ItemsSource is null)
            return;

        carousel.Item = carousel.ItemsSource[0]!;
    }

    private static void OnSelectedItemChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Carousel carousel)
            return;

        if (carousel.ItemsSource is null)
            return;

        carousel.Item = carousel.ItemsSource[carousel.SelectedItem]!;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RestartAnimation();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (_animation is null)
            return;

        _animation.Completed -= OnAnimationComplete;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        PauseAnimation();
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        ResumeAnimation();
    }

    private void OnAnimationComplete(object? sender, EventArgs e)
    {
        NextItem();
        RestartAnimation();
    }

    private void NextItem()
    {
        if (ItemsSource is null)
            return;

        int nextSlide = SelectedItem + 1;

        SelectedItem = nextSlide >= ItemsSource.Count ? 0 : nextSlide;
        RestartAnimation();
    }

    private void ResumeAnimation()
    {
        if (_animation is not null)
            _animation.Completed -= OnAnimationComplete;

        double totalMilliseconds = Duration.TimeSpan.TotalMilliseconds;
        double realDurationMillis = totalMilliseconds - (totalMilliseconds * ProgressPercentage);

        _animation = new DoubleAnimation(ProgressPercentage, 1, new Duration(TimeSpan.FromMilliseconds(realDurationMillis))) { FillBehavior = FillBehavior.HoldEnd };
        _animation.Completed += OnAnimationComplete;
        BeginAnimation(ProgressPercentageProperty, _animation, HandoffBehavior.SnapshotAndReplace);
    }

    private void PauseAnimation()
    {
        if (_animation is null)
            return;

        double currentValue = ProgressPercentage;
        BeginAnimation(ProgressPercentageProperty, null, HandoffBehavior.SnapshotAndReplace);
        ProgressPercentage = currentValue;
    }

    private void RestartAnimation()
    {
        if (_animation is not null)
            _animation.Completed -= OnAnimationComplete;

        _animation = new DoubleAnimation(0, 1, Duration) { FillBehavior = FillBehavior.HoldEnd };

        _animation.Completed += OnAnimationComplete;
        ProgressPercentage = 0;

        if (IsMouseOver)
            return;

        BeginAnimation(ProgressPercentageProperty, _animation, HandoffBehavior.SnapshotAndReplace);
    }

    private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Carousel carousel)
            return;

        carousel.RestartAnimation();
    }
}