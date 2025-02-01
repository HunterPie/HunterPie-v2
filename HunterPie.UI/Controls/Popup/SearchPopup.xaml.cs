using HunterPie.UI.Controls.Popup.Events;
using HunterPie.UI.Controls.TextBox.Events;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Controls.Popup;
/// <summary>
/// Interaction logic for SearchPopup.xaml
/// </summary>
public partial class SearchPopup : UserControl
{
    public delegate void RoutedSearchEventHandler(object sender, RoutedSearchEventArgs e);

    public static readonly RoutedEvent SearchEvent =
        EventManager.RegisterRoutedEvent(nameof(Search), RoutingStrategy.Bubble, typeof(RoutedSearchEventHandler),
            typeof(SearchPopup));

    public event RoutedSearchEventHandler Search
    {
        add => AddHandler(SearchEvent, value);
        remove => RemoveHandler(SearchEvent, value);
    }

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }
    public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(SearchPopup), new PropertyMetadata(false));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(SearchPopup));

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(SearchPopup));

    public double MaxResultsHeight
    {
        get => (double)GetValue(MaxResultsHeightProperty);
        set => SetValue(MaxResultsHeightProperty, value);
    }
    public static readonly DependencyProperty MaxResultsHeightProperty =
        DependencyProperty.Register(nameof(MaxResultsHeight), typeof(double), typeof(SearchPopup), new PropertyMetadata(double.NaN));

    public SearchPopup()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Escape)
            return;

        e.Handled = true;
        IsOpen = false;
    }

    private void OnPopupOpen(object sender, EventArgs e)
    {
        if (Window.GetWindow(PART_SearchBox) is not { } window)
            return;

        window.PreviewKeyDown += OnKeyDown;
    }

    private void OnPopupClose(object sender, EventArgs e)
    {
        if (Window.GetWindow(PART_SearchBox) is not { } window)
            return;

        window.PreviewKeyDown -= OnKeyDown;
    }

    private void OnSearch(object sender, SearchTextChangedEventArgs e)
    {
        RaiseEvent(
            new RoutedSearchEventArgs(SearchEvent, this)
            {
                Query = e.Text
            }
        );
    }
}