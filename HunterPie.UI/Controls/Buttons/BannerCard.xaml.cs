using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Buttons;
/// <summary>
/// Interaction logic for BannerCard.xaml
/// </summary>
public partial class BannerCard : UserControl
{

    public string Banner
    {
        get => (string)GetValue(BannerProperty);
        set => SetValue(BannerProperty, value);
    }

    // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BannerProperty =
        DependencyProperty.Register("Banner", typeof(string), typeof(BannerCard));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(BannerCard));

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(BannerCard));

    public string Link
    {
        get => (string)GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }

    // Using a DependencyProperty as the backing store for Link.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LinkProperty =
        DependencyProperty.Register("Link", typeof(string), typeof(BannerCard));

    public BannerCard()
    {
        InitializeComponent();
    }

    private void OnCardClick(object sender, RoutedEventArgs e) => Process.Start("explorer", Link);
}