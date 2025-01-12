using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Settings.Views;
/// <summary>
/// Interaction logic for ConfigurationPropertyView.xaml
/// </summary>
public partial class ConfigurationPropertyView : UserControl
{
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(ConfigurationPropertyView), new PropertyMetadata(string.Empty));

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(string), typeof(ConfigurationPropertyView), new PropertyMetadata(string.Empty));

    public bool RequiresRestart
    {
        get => (bool)GetValue(RequiresRestartProperty);
        set => SetValue(RequiresRestartProperty, value);
    }
    public static readonly DependencyProperty RequiresRestartProperty =
        DependencyProperty.Register(nameof(RequiresRestart), typeof(bool), typeof(ConfigurationPropertyView), new PropertyMetadata(false));

    public ConfigurationPropertyView()
    {
        InitializeComponent();
    }
}