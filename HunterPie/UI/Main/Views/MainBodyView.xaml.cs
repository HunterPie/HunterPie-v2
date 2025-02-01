using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Main.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Main.Views;

/// <summary>
/// Interaction logic for MainBodyView.xaml
/// </summary>
public partial class MainBodyView : UserControl
{
    private static readonly DoubleAnimation ScaleDownAnimation =
        new(1.5, 1, TimeSpan.FromMilliseconds(200)) { EasingFunction = new QuarticEase() };

    private static readonly DoubleAnimation FadeInAnimation = new(0, 1, TimeSpan.FromMilliseconds(500))
    {
        EasingFunction = new SineEase()
    };


    public MainBodyView()
    {
        InitializeComponent();
    }

    private void OnNavigationTargetUpdated(object? sender, DataTransferEventArgs e)
    {
        PART_NavigationHost.BeginAnimation(OpacityProperty, FadeInAnimation);
        PART_NavigationHost.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleDownAnimation);
        PART_NavigationHost.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleDownAnimation);
    }

    private void OnLaunchButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainBodyViewModel vm)
            return;

        vm.LaunchGame();
    }

    private async void OnCloseSupporterFeedback(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainBodyViewModel vm)
            return;

        await vm.CloseSupporterPromptAsync();
    }

    private void OnBannerClick(object sender, MouseButtonEventArgs e)
    {
        BrowserService.OpenUrl(CommonLinks.PATREON);
    }
}