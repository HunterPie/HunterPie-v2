namespace HunterPie.Core.Domain.Dialog;

public interface INativeDialogFactory
{
    public INativeDialog CreateDialog(NativeDialogType type, string title, string description, NativeDialogButtons buttons);
}