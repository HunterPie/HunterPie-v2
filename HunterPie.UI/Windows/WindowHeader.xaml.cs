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
public partial class WindowHeader : UserControl, INotifyPropertyChanged
{
    private Window _owner;
    private bool _isMouseDown;

    public event PropertyChangedEventHandler PropertyChanged;

    public bool IsMouseDown
    {
        get => _isMouseDown;
        private set
        {
            if (value != _isMouseDown)
            {
                _isMouseDown = value;
                this.N(PropertyChanged);
            }
        }
    }

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

    public WindowHeader()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void OnCloseButtonClick(object sender, EventArgs e) => Owner.Close();

    private void OnMinimizeButtonClick(object sender, EventArgs e) => Owner.WindowState = WindowState.Minimized;

    private void OnLeftMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsMouseDown = true;
        Owner.DragMove();
        IsMouseDown = false;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => Owner = Window.GetWindow(this);
}