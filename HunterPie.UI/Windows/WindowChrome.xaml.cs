using HunterPie.UI.Architecture.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Windows;

/// <summary>
/// Interaction logic for WindowHeader.xaml
/// </summary>
public partial class WindowChrome : UserControl, INotifyPropertyChanged
{
    private Window _owner;
    public Window Owner
    {
        get => _owner;
        private set
        {
            if (value != _owner)
            {
                _owner = value;
                this.N(PropertyChanged);
            }
        }
    }

    public object Container
    {
        get => GetValue(ContainerProperty);
        set => SetValue(ContainerProperty, value);
    }
    public static readonly DependencyProperty ContainerProperty =
        DependencyProperty.Register("Container", typeof(object), typeof(WindowChrome));

    public event PropertyChangedEventHandler PropertyChanged;

    public WindowChrome()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClick(object sender, EventArgs e) => Owner.Close();

    private void OnMinimizeButtonClick(object sender, EventArgs e) => Owner.WindowState = WindowState.Minimized;

    private void OnLeftMouseDown(object sender, MouseButtonEventArgs e) => Owner.DragMove();

    private void OnLoaded(object sender, RoutedEventArgs e) => Owner = Window.GetWindow(this);
}