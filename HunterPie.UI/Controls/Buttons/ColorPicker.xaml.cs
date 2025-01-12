using HunterPie.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using ColorDialog = System.Windows.Forms.ColorDialog;
using DialogResult = System.Windows.Forms.DialogResult;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for ColorPicker.xaml
/// </summary>
public partial class ColorPicker : UserControl
{
    public string Color
    {
        get => (string)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(string), typeof(ColorPicker), new PropertyMetadata("#00000000"));

    public ColorPicker()
    {
        InitializeComponent();
    }

    private void OnButtonClick(object sender, EventArgs e)
    {
        using ColorDialog colorDialog = new();

        if (colorDialog.ShowDialog() != DialogResult.OK)
            return;

        Color = colorDialog.Color.ToHexString();
    }
}