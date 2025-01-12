using System;

namespace HunterPie.Core.Game.Events;

public class SimpleValueChangeEventArgs<T> : EventArgs
{
    public T OldValue { get; }
    public T NewValue { get; }

    public SimpleValueChangeEventArgs(T oldValue, T newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}