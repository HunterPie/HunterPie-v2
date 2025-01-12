namespace HunterPie.UI.Architecture.Events;

public delegate void DataRoutedEventHandler<T>(object sender, DataRoutedEventArgs<T> e);