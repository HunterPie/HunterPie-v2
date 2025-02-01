using System;

namespace HunterPie.Core.Domain.Dialog;

[Flags]
public enum NativeDialogButtons
{
    Accept = 1 << 0,
    Reject = 1 << 1,
    Cancel = 1 << 2
}