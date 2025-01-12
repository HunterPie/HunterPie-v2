using HunterPie.Core.Remote;
using HunterPie.DI;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Account.ViewModels;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Navigation;

namespace HunterPie.UI.Header.ViewModels;

internal class AccountMenuViewModel : ViewModel
{
    private readonly IAccountUseCase _accountUseCase;

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set => SetValue(ref _isLoading, value); }

    private string _avatarUrl = string.Empty;
    public string AvatarUrl { get => _avatarUrl; set => SetValue(ref _avatarUrl, value); }

    private string _username = string.Empty;
    public string Username { get => _username; set => SetValue(ref _username, value); }

    private bool _isLoggedIn;
    public bool IsLoggedIn { get => _isLoggedIn; set => SetValue(ref _isLoggedIn, value); }

    private bool _isOpen;

    public AccountMenuViewModel(
        IAccountUseCase accountUseCase
    )
    {
        _accountUseCase = accountUseCase;
    }

    public bool IsOpen { get => _isOpen; set => SetValue(ref _isOpen, value); }

    public void OpenSignInScreen()
    {
        AccountSignFlowViewModel vm = DependencyContainer.Get<AccountSignFlowViewModel>();

        Navigator.App.Navigate(vm);
    }

    public void OpenAccountSettings()
    {
        Navigator.App.Navigate<SettingsViewModel>();
    }

    public async void OpenAccountDetails()
    {
        AccountPreferencesViewModel viewModel = DependencyContainer.Get<AccountPreferencesViewModel>()
            .Apply(it => it.IsFetchingAccount = true);
        UserAccount? account = await _accountUseCase.GetAsync();

        if (account is not null)
            await viewModel.ApplyAsync(async it =>
            {
                it.Email = account.Email;
                it.Username = account.Username;
                it.AvatarUrl = await CDN.GetAsset(account.AvatarUrl);
                it.IsSupporter = account.IsSupporter;
                it.IsFetchingAccount = false;
            });


        Navigator.Body.Navigate(viewModel);
    }

    public async void SignOut()
    {
        IsLoggedIn = false;
        await _accountUseCase.LogoutAsync();
    }
}