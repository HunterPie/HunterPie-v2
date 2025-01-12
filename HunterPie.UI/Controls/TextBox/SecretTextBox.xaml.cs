using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.TextBox;

/// <summary>
/// Interaction logic for SecretTextBox.xaml
/// </summary>
public partial class SecretTextBox : UserControl
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(SecretTextBox), new(string.Empty, OnValueChange));

    public bool IsContentVisible
    {
        get => (bool)GetValue(IsContentVisibleProperty);
        set => SetValue(IsContentVisibleProperty, value);
    }

    public static readonly DependencyProperty IsContentVisibleProperty =
        DependencyProperty.Register("IsContentVisible", typeof(bool), typeof(SecretTextBox), new(false));

    public SecretTextBox()
    {
        InitializeComponent();
    }

    private void OnHideButtonClick(object sender, EventArgs e) => IsContentVisible = !IsContentVisible;

    private void OnPasswordChanged(object sender, RoutedEventArgs e) => Text = PART_PasswordBox.Password;

    private static void OnValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SecretTextBox secretTextBox)
        {
            string value = e.NewValue as string;
            if (secretTextBox.PART_PasswordBox.Password == value)
                return;

            secretTextBox.PART_PasswordBox.Password = value;
        }
    }
}