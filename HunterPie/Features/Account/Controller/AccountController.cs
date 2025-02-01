using HunterPie.Core.Remote;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Header.ViewModels;
using HunterPie.UI.Main.Navigators;
using System;

namespace HunterPie.Features.Account.Controller;

internal class AccountController
{
    private readonly MainBodyNavigator _mainBodyNavigator;
    private readonly IAccountUseCase _accountUseCase;
    private readonly AccountMenuViewModel _menuViewModel;

    public AccountController(
        IAccountUseCase accountUseCase,
        AccountMenuViewModel menuViewModel,
        MainBodyNavigator mainBodyNavigator)
    {
        _mainBodyNavigator = mainBodyNavigator;
        _accountUseCase = accountUseCase;
        _menuViewModel = menuViewModel.Apply(it => it.IsLoading = true);

        Subscribe();
    }

    private void Subscribe()
    {
        _accountUseCase.SignIn += OnSignIn;
        _accountUseCase.SessionStart += OnSessionStart;
        _accountUseCase.SignOut += OnSignOut;
        _accountUseCase.AvatarChange += OnAvatarChange;
    }

    private async void OnAvatarChange(object? sender, AccountAvatarEventArgs e)
    {
        _menuViewModel.AvatarUrl = await CDN.GetAsset(e.AvatarUrl);
    }

    private void OnSignOut(object? sender, EventArgs e)
    {
        _menuViewModel.IsLoggedIn = false;
        _menuViewModel.IsLoading = false;

        _mainBodyNavigator.ReturnWhen<AccountPreferencesViewModel>();
    }

    private void OnSessionStart(object? sender, AccountLoginEventArgs e) => UpdateViewModels(e.Account);

    private void OnSignIn(object? sender, AccountLoginEventArgs e) => UpdateViewModels(e.Account);

    private async void UpdateViewModels(UserAccount account)
    {
        _menuViewModel.Username = account.Username;
        _menuViewModel.AvatarUrl = await CDN.GetAsset(account.AvatarUrl);
        _menuViewModel.IsLoggedIn = true;
        _menuViewModel.IsLoading = false;
    }
}