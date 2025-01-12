using HunterPie.Core.Extensions;
using HunterPie.Domain.Sidebar;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.UI.Main.Navigators.Events;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.UI.Main.Navigators;

internal class NavigatorController
{
    private readonly ISideBarCollection _sideBar;
    private readonly IAppNavigationDispatcher _mainNavigationDispatcher;
    private readonly IAppNavigator _appNavigator;
    private readonly IBodyNavigationDispatcher _bodyNavigationDispatcher;
    private readonly IBodyNavigator _bodyNavigator;
    private readonly IAccountUseCase _accountUseCase;
    private readonly MainViewModel _mainViewModel;
    private readonly MainBodyViewModel _mainBodyViewModel;

    public NavigatorController(
        ISideBarCollection sideBar,
        IAppNavigationDispatcher mainNavigationDispatcher,
        IAppNavigator appNavigator,
        IBodyNavigationDispatcher bodyNavigationDispatcher,
        IBodyNavigator bodyNavigator,
        IAccountUseCase accountUseCase,
        MainViewModel mainViewModel,
        MainBodyViewModel mainBodyViewModel)
    {
        _sideBar = sideBar;
        _mainNavigationDispatcher = mainNavigationDispatcher;
        _bodyNavigationDispatcher = bodyNavigationDispatcher;
        _bodyNavigator = bodyNavigator;
        _accountUseCase = accountUseCase;
        _appNavigator = appNavigator;
        _mainViewModel = mainViewModel;
        _mainBodyViewModel = mainBodyViewModel;

        Subscribe();
    }

    public async Task SetupAsync()
    {
        _appNavigator.Navigate(
            viewModel: _mainBodyViewModel
        );

        if (_sideBar.Elements.FirstOrDefault() is not { } sideBarElement)
            return;

        await sideBarElement.ExecuteAsync();
    }

    private void Subscribe()
    {
        _mainNavigationDispatcher.NavigateRequest +=
            (_, args) => _mainViewModel.ContentViewModel = args.ViewModel;
        _bodyNavigationDispatcher.NavigateRequest += BodyNavigateRequest;
        _accountUseCase.SessionStart += (_, e) => SetupViewModel(e.Account);
        _accountUseCase.SignIn += (_, e) => SetupViewModel(e.Account);
        _accountUseCase.SignOut += (_, _) => SetupViewModel(null);
    }

    private void BodyNavigateRequest(object? sender, NavigateRequestEventArgs e)
    {
        _mainBodyViewModel.NavigationViewModel = e.ViewModel;

        Type viewModelType = e.ViewModel.GetType();

        _sideBar.Elements.ForEach(element => element.IsSelected = viewModelType == element.Type);
    }

    private async void SetupViewModel(UserAccount? account)
    {
        await _mainBodyViewModel.InitializeSupporterPromptAsync(account?.IsSupporter ?? false);
    }
}