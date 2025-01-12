using System.Diagnostics;

namespace HunterPie.Core.System;
public static class BrowserService
{
    public static void OpenUrl(string url) =>
        Process.Start("explorer", url);

    public static void OpenFolder(string path) =>
        Process.Start("explorer", path);
}