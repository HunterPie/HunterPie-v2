using System.Windows;
using System.Windows.Input;

namespace HunterPie.Update.Presentation;

/// <summary>
/// Interaction logic for UpdateView.xaml
/// </summary>
public partial class UpdateView : Window
{

    public bool IsMouseDown
    {
        get => (bool)GetValue(IsMouseDownProperty);
        set => SetValue(IsMouseDownProperty, value);
    }
    public static readonly DependencyProperty IsMouseDownProperty =
        DependencyProperty.Register("IsMouseDown", typeof(bool), typeof(UpdateView), new PropertyMetadata(false));

    public UpdateView()
    {
        InitializeComponent();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsMouseDown = true;
        DragMove();
        IsMouseDown = false;
    }
}