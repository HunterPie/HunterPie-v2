using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Header.Views;
/// <summary>
/// Interaction logic for HeaderView.xaml
/// </summary>
public partial class HeaderView : UserControl
{
    public static readonly RoutedEvent CloseClickEvent = EventManager.RegisterRoutedEvent(
        nameof(CloseClick),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(HeaderView)
    );

    public event RoutedEventHandler CloseClick
    {
        add => AddHandler(CloseClickEvent, value);
        remove => RemoveHandler(CloseClickEvent, value);
    }

    public static readonly RoutedEvent MinimizeClickEvent = EventManager.RegisterRoutedEvent(
        nameof(MinimizeClick),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(HeaderView)
    );

    public event RoutedEventHandler MinimizeClick
    {
        add => AddHandler(MinimizeClickEvent, value);
        remove => RemoveHandler(MinimizeClickEvent, value);
    }

    public static readonly RoutedEvent DragStartEvent = EventManager.RegisterRoutedEvent(
        nameof(DragStart),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(HeaderView)
    );

    public event RoutedEventHandler DragStart
    {
        add => AddHandler(DragStartEvent, value);
        remove => RemoveHandler(DragStartEvent, value);
    }

    public HeaderView()
    {
        InitializeComponent();
    }

    private void OnCloseClick(object sender, RoutedEventArgs e) =>
        RaiseEvent(new RoutedEventArgs(CloseClickEvent, this));

    private void OnMinimizeClick(object sender, RoutedEventArgs e) =>
        RaiseEvent(new RoutedEventArgs(MinimizeClickEvent, this));

    private void OnMouseLeftDown(object sender, MouseButtonEventArgs e) =>
        RaiseEvent(new RoutedEventArgs(DragStartEvent, this));
}