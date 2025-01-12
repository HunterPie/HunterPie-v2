using System.Windows;

namespace HunterPie.UI.Controls.Paginating.Events;

public class PaginationEventArgs : RoutedEventArgs
{
    /// <summary>
    /// The page that was clicked
    /// </summary>
    public int Page { get; }

    public PaginationEventArgs(RoutedEvent routedEvent, object sender, int page) : base(routedEvent, sender)
    {
        Page = page;
    }
}