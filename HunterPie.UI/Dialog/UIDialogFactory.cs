using HunterPie.Core.Domain.Dialog;

namespace HunterPie.UI.Dialog;

internal class UIDialogFactory : INativeDialogFactory
{
    public INativeDialog CreateDialog(NativeDialogType type, string title, string description, NativeDialogButtons buttons)
    {
        INativeDialog dialog = new DialogView();
        switch (type)
        {
            case NativeDialogType.Warn:
                dialog.Warn(title, description, buttons);
                break;
            case NativeDialogType.Error:
                dialog.Error(title, description, buttons);
                break;
            case NativeDialogType.Info:
                dialog.Info(title, description, buttons);
                break;
        }

        return dialog;
    }
}