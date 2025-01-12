using HunterPie.Core.Domain.Interfaces;
using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace HunterPie.Features.Account.Views;

/// <summary>
/// Interaction logic for AccountSignFlowView.xaml
/// </summary>
internal partial class AccountSignFlowView : View<AccountSignFlowViewModel>, IEventDispatcher
{
    private readonly ThicknessAnimation[] _animations = {
        new(new Thickness(12, 12, 0, 10), TimeSpan.FromMilliseconds(200), FillBehavior.HoldEnd),
        new(new Thickness(198, 12, 0, 10), TimeSpan.FromMilliseconds(200), FillBehavior.HoldEnd),
    };
    private Storyboard? _slideOutAnimation;

    public AccountSignFlowView()
    {
        InitializeComponent();
    }

    public override void EndInit()
    {
        base.EndInit();
        _slideOutAnimation = FindResource("SB_SLIDE_OUT") as Storyboard;
    }

    private void CloseForm()
    {
        ViewModel.NavigateBack();
        Dispose();
    }

    private void OnCloseClick(object sender, RoutedEventArgs e) => AnimateSlideOut();

    private void AnimateSlide(int index)
    {
        PART_SelectedSignFlowHighlight.BeginAnimation(FrameworkElement.MarginProperty, _animations[index]);
    }

    private void AnimateSlideOut() => _slideOutAnimation?.Begin(PART_Border);

    private void OnSlideOutCompleted(object sender, EventArgs e) => CloseForm();

    private void OnTabUpdate(object? sender, DataTransferEventArgs e)
    {
        AnimateSlide(ViewModel.SelectedIndex);
    }
}