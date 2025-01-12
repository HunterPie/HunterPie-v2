using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.UI.Architecture;
using HunterPie.UI.Main.Navigators.Events;
using HunterPie.UI.Navigation;
using System;
using System.Collections.Generic;

namespace HunterPie.UI.Main.Navigators;

internal class MainNavigator : IAppNavigator, IAppNavigationDispatcher, IEventDispatcher
{
    private readonly Dictionary<Type, ViewModel> _viewModels = new();
    private readonly Stack<ViewModel> _stack = new();

    public event EventHandler<NavigateRequestEventArgs>? NavigateRequest;

    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
    {
        Type viewModelType = viewModel.GetType();

        if (_viewModels.ContainsKey(viewModelType))
            _viewModels.Remove(viewModelType);

        _viewModels.Add(viewModelType, viewModel);

        this.Dispatch(
            toDispatch: NavigateRequest,
            data: new NavigateRequestEventArgs
            {
                ViewModel = viewModel
            });
    }

    public void Navigate<TViewModel>() where TViewModel : ViewModel
    {
        if (!_viewModels.ContainsKey(typeof(TViewModel)))
            return;

        Navigate(_viewModels[typeof(TViewModel)]);
    }

    public void Return()
    {
        _stack.PopOrDefault();

        if (!_stack.TryPop(out ViewModel? viewModel))
            return;

        Navigate(viewModel);
    }

    public void ReturnWhen<TViewModel>() where TViewModel : ViewModel
    {
        if (_stack.TryPeek(out ViewModel? vm) && vm is TViewModel)
            Return();
    }

}