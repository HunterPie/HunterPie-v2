using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture;
using System;
using System.Windows;
using System.Windows.Input;

namespace HunterPie.Features.Account.Views;
/// <summary>
/// Interaction logic for AccountLoginFlowView.xaml
/// </summary>
internal partial class AccountLoginFlowView : View<AccountLoginFlowViewModel>
{
    public AccountLoginFlowView()
    {
        InitializeComponent();
    }

    private async void OnSignInClick(object sender, EventArgs e)
    {
        if (!await ViewModel.SignIn())
            return;
    }

    private async void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        _ = await ViewModel.SignIn();
    }

    private void OnForgotPasswordClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToPasswordResetFlow();

    private void OnResendVerificationClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToAccountVerificationFlow();
}