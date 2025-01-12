using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Text;
/// <summary>
/// Interaction logic for Title.xaml
/// </summary>
public partial class Title : UserControl
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(Title), new PropertyMetadata(string.Empty));


    public Title()
    {
        InitializeComponent();
    }
}