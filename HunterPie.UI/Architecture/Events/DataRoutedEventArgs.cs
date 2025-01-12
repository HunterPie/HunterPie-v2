using System.Windows;

namespace HunterPie.UI.Architecture.Events;

public class DataRoutedEventArgs<T> : RoutedEventArgs
{
    public T Data { get; }

    public DataRoutedEventArgs(RoutedEvent routedEvent, object source, T data) : base(routedEvent, source)
    {
        Data = data;
    }
}