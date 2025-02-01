namespace HunterPie.Core.Domain.Dialog;

/// <summary>
/// HunterPie's native dialog manager
/// </summary>
public class DialogManager
{
    private readonly INativeDialogFactory _factory;
    private static DialogManager _instance;

    internal DialogManager(INativeDialogFactory factory)
    {
        _factory = factory;
        _instance = this;
    }

    public static NativeDialogResult Warn(string title, string description, NativeDialogButtons buttons) => _instance._factory.CreateDialog(NativeDialogType.Warn, title, description, buttons).DialogResult();

    public static NativeDialogResult Info(string title, string description, NativeDialogButtons buttons) => _instance._factory.CreateDialog(NativeDialogType.Info, title, description, buttons).DialogResult();

    public static NativeDialogResult Error(string title, string description, NativeDialogButtons buttons) => _instance._factory.CreateDialog(NativeDialogType.Error, title, description, buttons).DialogResult();
}