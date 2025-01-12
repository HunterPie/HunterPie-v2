using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Text;
/// <summary>
/// Interaction logic for LabeledText.xaml
/// </summary>
public partial class LabeledText : UserControl
{
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register("Label", typeof(string), typeof(LabeledText), new PropertyMetadata("Label"));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(LabeledText), new PropertyMetadata("Sample Text"));

    public LabeledText()
    {
        InitializeComponent();
    }
}