using System.ComponentModel;
using System.Diagnostics;

namespace HunterPie.UI.Architecture.Utils;
public static class Development
{
    public static bool IsDesignMode()
    {
        bool isWpfDesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        using var self = Process.GetCurrentProcess();
        bool isFormsDesignMode = self.ProcessName == "devenv.exe";

        return isWpfDesignMode || isFormsDesignMode;
    }
}