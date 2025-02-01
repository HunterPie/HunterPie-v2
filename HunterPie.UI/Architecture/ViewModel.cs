using HunterPie.Core.Architecture;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.UI.Architecture;

public class ViewModel : Bindable
{
    public Dispatcher UIThread => Application.Current.Dispatcher;

    protected void SetValueThenExecute<T>(
        ref T property,
        T value,
        Action action,
        [CallerMemberName] string propertyName = "")
    {
        SetValue(ref property, value, propertyName);
        action();
    }
}