using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;

namespace HunterPie.Core.Client;

public static class ClientInfo
{
    public static string ClientFileName
    {
        get
        {
            using var selfProcess = Process.GetCurrentProcess();

            return selfProcess.MainModule.FileName;
        }
    }

    public static string ClientPath => AppDomain.CurrentDomain.BaseDirectory;

    public static string PluginsPath => Path.Combine(ClientPath, "Modules");

    public static string LanguagesPath => Path.Combine(ClientPath, "Languages");

    public static string AddressPath => Path.Combine(ClientPath, "Address");

    public static string ThemesPath => Path.Combine(ClientPath, "Themes");

    public static Version Version
    {
        get
        {
            var self = Assembly.GetEntryAssembly();
            AssemblyName name = self.GetName();
            return name.Version;
        }
    }

    public static bool IsAdmin
    {
        get
        {
            var winIdentity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(winIdentity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    public const string CONFIG_NAME = "config.json";

    public const string CONFIG_BACKUP_NAME = CONFIG_NAME + ".bak";

    public static bool IsVersionGreaterOrEq(Version other)
    {
        var self = Assembly.GetEntryAssembly();
        AssemblyName name = self.GetName();
        Version ver = name.Version;

        return ver >= other;
    }

    public static string GetPathFor(string relative) => Path.Combine(ClientPath, relative);

    public static string GetRandomTempFile() => Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

    public static string GetRandomTempDirectory() => Directory.CreateDirectory(
        Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
    ).FullName;
}