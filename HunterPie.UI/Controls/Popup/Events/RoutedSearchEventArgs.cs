using System.Windows;

namespace HunterPie.UI.Controls.Popup.Events;

public class RoutedSearchEventArgs : RoutedEventArgs
{
    public required string Query { get; init; }

    public RoutedSearchEventArgs(RoutedEvent e, object sender) : base(e, sender) { }
}