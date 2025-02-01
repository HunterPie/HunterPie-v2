using HunterPie.Core.Remote;
using HunterPie.DI;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.UI.Header.ViewModels;

internal class AccountMenuViewModel : ViewModel
{
    private readonly IAccountUseCase _accountUseCase;
    private readonly IAppNavigator _appNavigator;
    private readonly IBodyNavigator _bodyNavigator;
    private readonly SettingsSideBarViewModel _settingsSideBarViewModel;

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
        IAccountUseCase accountUseCase,
        IAppNavigator appNavigator,
        IBodyNavigator bodyNavigator,
        SettingsSideBarViewModel settingsSideBarViewModel)
    {
        _accountUseCase = accountUseCase;
        _appNavigator = appNavigator;
        _bodyNavigator = bodyNavigator;
        _settingsSideBarViewModel = settingsSideBarViewModel;
    }

    public bool IsOpen { get => _isOpen; set => SetValue(ref _isOpen, value); }

    public void OpenSignInScreen()
    {
        AccountSignFlowViewModel vm = DependencyContainer.Get<AccountSignFlowViewModel>();

        _appNavigator.Navigate(vm);
    }

    public async Task OpenAccountSettingsAsync()
    {
        await _settingsSideBarViewModel.ExecuteAsync();
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


        _bodyNavigator.Navigate(viewModel);
    }

    public async void SignOut()
    {
        IsLoggedIn = false;
        await _accountUseCase.LogoutAsync();
    }
}