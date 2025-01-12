using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.UI.Architecture;
using HunterPie.UI.Main.Navigators.Events;
using HunterPie.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.UI.Main.Navigators;

internal class MainBodyNavigator : IBodyNavigator, IBodyNavigationDispatcher, IEventDispatcher
{
    private readonly Stack<ViewModel> _stack = new();

    public event EventHandler<NavigateRequestEventArgs>? NavigateRequest;

    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
    {
        _stack.Push(viewModel);

        this.Dispatch(
            toDispatch: NavigateRequest,
            data: new NavigateRequestEventArgs
            {
                ViewModel = viewModel
            });
    }

    public void Navigate<TViewModel>() where TViewModel : ViewModel
    {
        ViewModel? viewModel = _stack.FirstOrDefault(it => it.GetType() == typeof(TViewModel));

        if (viewModel is not { })
            return;

        Navigate(viewModel);
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