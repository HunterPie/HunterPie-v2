using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Architecture;

public class ClickableControl : UserControl
{
    private bool _isMouseInside;
    private bool _isMouseDown;

    public event EventHandler<EventArgs> OnClick;

    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ClickableControl));

    public event RoutedEventHandler Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        _isMouseDown = true;
        e.Handled = true;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        // Was a click!
        if (_isMouseDown && _isMouseInside)
        {
            OnClickEvent();
            OnClick?.Invoke(this, e);
            RaiseEvent(new RoutedEventArgs(ClickEvent, this));
        }

        _isMouseDown = false;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        _isMouseInside = true;

    }
    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        _isMouseInside = false;
        _isMouseDown = false;
    }

    protected virtual void OnClickEvent()
    {

    }
}