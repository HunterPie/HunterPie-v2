using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TB = System.Windows.Controls.TextBox;

namespace HunterPie.UI.Controls.Sliders;

/// <summary>
/// Interaction logic for Range.xaml
/// </summary>
public partial class Range : UserControl
{
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(Range), new PropertyMetadata(0.0));

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(Range), new PropertyMetadata(0.0));

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }
    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(Range), new PropertyMetadata(0.0));

    public double Change
    {
        get => (double)GetValue(ChangeProperty);
        set => SetValue(ChangeProperty, value);
    }
    public static readonly DependencyProperty ChangeProperty =
        DependencyProperty.Register(nameof(Change), typeof(double), typeof(Range), new PropertyMetadata(1.0));

    public Range()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && sender is TB textbox)
            UpdateBinding(textbox);
    }

    private void OnLostFocus(object sender, RoutedEventArgs e) => UpdateBinding(sender as TB);

    private static void UpdateBinding(TB textbox)
    {
        BindingExpression binding = textbox.GetBindingExpression(TB.TextProperty);
        binding.UpdateSource();
    }
}