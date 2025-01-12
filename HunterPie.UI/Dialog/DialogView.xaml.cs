using HunterPie.Core.Domain.Dialog;
using HunterPie.UI.Architecture.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Dialog;

/// <summary>
/// Interaction logic for DialogView.xaml
/// </summary>
public partial class DialogView : Window, INativeDialog, INotifyPropertyChanged
{
    private NativeDialogButtons _buttons;

    private string _description = "This is a default dialog text";
    private string _dialogTitle;
    public new NativeDialogResult DialogResult { get; private set; }
    public NativeDialogButtons Buttons
    {
        get => _buttons;
        set
        {
            if (value != _buttons)
            {
                _buttons = value;
                this.N(PropertyChanged);
            }
        }
    }

    public string DialogTitle
    {
        get => _dialogTitle;
        set
        {
            if (value != _dialogTitle)
            {
                _dialogTitle = value;
                this.N(PropertyChanged);
            }
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (value != _description)
            {
                _description = value;
                this.N(PropertyChanged);
            }
        }
    }

    public DialogView()
    {
        InitializeComponent();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Warn(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_WARN", buttons);

    public void Info(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_INFO", buttons);

    public void Error(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_ERROR", buttons);

    private void SetDialogInfo(string title, string description, string icon, NativeDialogButtons buttons)
    {
        DialogTitle = title;
        Description = description;
        Icon = FindResource(icon) as ImageSource;
        Buttons = buttons;
        _ = ShowDialog();
    }

    NativeDialogResult INativeDialog.DialogResult() => DialogResult;

    private void OnAccept(object sender, EventArgs e)
    {
        DialogResult = NativeDialogResult.Accept;
        Close();
    }

    private void OnReject(object sender, EventArgs e)
    {
        DialogResult = NativeDialogResult.Reject;
        Close();
    }

    private void OnCancel(object sender, EventArgs e)
    {
        DialogResult = NativeDialogResult.Cancel;
        Close();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);

        if (DialogResult == NativeDialogResult.NotFinished)
            DialogResult = NativeDialogResult.Cancel;
    }
}