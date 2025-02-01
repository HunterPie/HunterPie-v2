using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;
using System.Windows.Input;

namespace HunterPie.Features.Account.Views;
/// <summary>
/// Interaction logic for AccountRegisterFlowView.xaml
/// </summary>
internal partial class AccountRegisterFlowView : View<AccountRegisterFlowViewModel>
{
    public AccountRegisterFlowView()
    {
        InitializeComponent();
    }

    private void OnRegisterClick(object sender, RoutedEventArgs e) => ViewModel.SignUp();

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        ViewModel.SignUp();
    }
}