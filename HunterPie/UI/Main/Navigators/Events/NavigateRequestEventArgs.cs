using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.Main.Navigators.Events;

internal class NavigateRequestEventArgs : EventArgs
{
    public required ViewModel ViewModel { get; init; }
}