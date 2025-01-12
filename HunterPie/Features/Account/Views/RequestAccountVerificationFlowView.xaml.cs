using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.Features.Account.Views;
/// <summary>
/// Interaction logic for RequestAccountVerificationFlowView.xaml
/// </summary>
internal partial class RequestAccountVerificationFlowView : View<AccountVerificationResendFlowViewModel>
{
    public RequestAccountVerificationFlowView()
    {
        InitializeComponent();
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToLoginFlow();

    private async void OnResendVerificationButtonClick(object sender, RoutedEventArgs e) => await ViewModel.RequestAccountVerification();
}