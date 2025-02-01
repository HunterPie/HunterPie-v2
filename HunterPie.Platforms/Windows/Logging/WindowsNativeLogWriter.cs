using HunterPie.Platforms.Common.Logging;
using HunterPie.Platforms.Windows.Api.Kernel;

namespace HunterPie.Platforms.Windows.Logging;

internal class WindowsNativeLogWriter : INativeLogWriter
{
    private enum LogType
    {
        Benchmark = ConsoleColor.Gray,
        Debug = ConsoleColor.DarkGreen,
        Warning = ConsoleColor.DarkYellow,
        Error = ConsoleColor.DarkRed,
        Info = ConsoleColor.DarkCyan,
        Native = ConsoleColor.Magenta
    }

    public void CreateTerminal()
    {
        Kernel32.AllocConsole();
        Console.Title = "[DEBUG] HunterPie Console";
    }

    public void Debug(string message) =>
        WriteToStdout(LogType.Debug, message);

    public void Info(string message) =>
        WriteToStdout(LogType.Info, message);

    public void Warning(string message) =>
        WriteToStdout(LogType.Warning, message);

    public void Native(string message) =>
        WriteToStdout(LogType.Native, message);

    public void Error(string message) =>
        WriteToStdout(LogType.Error, message);

    public void Benchmark(string message) =>
        WriteToStdout(LogType.Benchmark, message);

    private static void WriteToStdout(LogType type, string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write($"[{DateTime.Now.ToLongTimeString()}]");
        Console.ForegroundColor = (ConsoleColor)type;
        Console.Write($"[{type.ToString().ToUpper()}] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
    }
}