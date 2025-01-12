using HunterPie.UI.Architecture.Utils;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Controls.Buttons;
/// <summary>
/// Interaction logic for ComboButton.xaml
/// </summary>
public partial class ComboButton : UserControl
{
    public new object Content
    {
        get => (object)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static new readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(
            nameof(Content),
            typeof(object),
            typeof(ComboButton)
        );

    public bool IsDropDownOpen
    {
        get => (bool)GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public static readonly DependencyProperty IsDropDownOpenProperty =
        DependencyProperty.Register(
            nameof(IsDropDownOpen),
            typeof(bool),
            typeof(ComboButton),
            new PropertyMetadata(
                false,
                new PropertyChangedCallback(OnIsDropDownOpenChanged)
            )
        );

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ComboButton));

    public object SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.Register(nameof(SelectedValue), typeof(object), typeof(ComboButton));

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(ComboButton));

    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ComboButton));

    public event RoutedEventHandler Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    public ComboButton()
    {
        InitializeComponent();
    }

    private void OnExpandPopupClick(object sender, RoutedEventArgs e)
    {
        IsDropDownOpen = !IsDropDownOpen;
    }

    private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var comboButton = (ComboButton)d;

        bool isDropDownOpen = (bool)e.NewValue;

        if (isDropDownOpen)
            Mouse.Capture(comboButton, CaptureMode.SubTree);
        else
            Mouse.Capture(null);
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (Mouse.Captured is null)
            return;

        Point buttonDistance = e.GetPosition(PART_ExpandButton);
        bool wasClickedOnExpander = buttonDistance.IsWithinBounds(PART_ExpandButton);

        if (wasClickedOnExpander)
            return;

        IsDropDownOpen = false;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this);

        if (parentWindow is null)
            return;

        parentWindow.Deactivated += (_, _) => IsDropDownOpen = false;
    }

    private void OnClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(ClickEvent, this));
}