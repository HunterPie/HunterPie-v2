using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for TabItem.xaml
/// </summary>
public partial class TabItem : UserControl
{
    private readonly Storyboard _rippleAnimation;

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(TabItem), new PropertyMetadata("Header"));

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(TabItem), new PropertyMetadata("Header tooltip"));

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TabItem));

    public TabItem()
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
    }
}