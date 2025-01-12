using HunterPie.Features.Account.UseCase;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountLoginFlowViewModel : ViewModel
{
    private string _email = "";
    private string _password = "";
    private bool _canLogIn;
    private bool _isLoggingIn;

    public string Email
    {
        get => _email;
        set
        {
            CanLogIn = Password.Length > 0 && value.Length > 0;
            SetValue(ref _email, value);
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            CanLogIn = Email.Length > 0 && value.Length > 0;
            SetValue(ref _password, value);
        }
    }
    public bool IsLoggingIn { get => _isLoggingIn; set => SetValue(ref _isLoggingIn, value); }
    public bool CanLogIn { get => _canLogIn; set => SetValue(ref _canLogIn, value); }

    private readonly IAccountUseCase _accountUseCase;
    public AccountPasswordResetFlowViewModel PasswordResetFlowViewModel { get; }
    public AccountVerificationResendFlowViewModel AccountVerificationResendFlowViewModel { get; }

    public AccountLoginFlowViewModel(
        AccountPasswordResetFlowViewModel passwordResetFlowViewModel,
        IAccountUseCase accountUseCase,
        AccountVerificationResendFlowViewModel accountVerificationResendFlowViewModel)
    {
        PasswordResetFlowViewModel = passwordResetFlowViewModel;
        _accountUseCase = accountUseCase;
        AccountVerificationResendFlowViewModel = accountVerificationResendFlowViewModel;
    }

    public async Task<bool> SignIn()
    {
        if (!CanLogIn)
            return false;

        IsLoggingIn = true;
        CanLogIn = false;

        var request = new LoginRequest
        (
            Email: Email,
            Password: Password
        );
        PoogieResult<LoginResponse>? result = await _accountUseCase.LoginAsync(request);

        IsLoggingIn = false;
        CanLogIn = true;

        if (result is null || result.Error is { })
            return false;

        Navigator.App.Navigate<MainBodyViewModel>();

        return true;
    }

    public void NavigateToPasswordResetFlow() => PasswordResetFlowViewModel.IsFlowActive = true;

    public void NavigateToAccountVerificationFlow() => AccountVerificationResendFlowViewModel.IsFlowActive = true;
}