using System;

namespace HunterPie.Core.Game.Events;

public class StateChangeEventArgs<T> : EventArgs
{
    public T State { get; }
    public T PreviousState { get; }

    public StateChangeEventArgs(T state, T previousState)
    {
        State = state;
        PreviousState = previousState;
    }
}