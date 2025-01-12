using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Display;
/// <summary>
/// Interaction logic for Section.xaml
/// </summary>
public partial class Section : UserControl
{
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(Section));

    public new object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    public static new readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(object), typeof(Section));

    public Section()
    {
        InitializeComponent();
    }
}