using HunterPie.UI.Main.Navigators.Events;
using System;

namespace HunterPie.UI.Main.Navigators;

internal interface INavigationDispatcher
{
    public event EventHandler<NavigateRequestEventArgs> NavigateRequest;
}