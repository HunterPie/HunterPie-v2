using HunterPie.UI.Logger;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Logging.ViewModels;

internal class MockConsoleViewModel
{
    public ObservableCollection<LogString> Logs { get; } = new();

    public MockConsoleViewModel()
    {
        Logs.Add(new()
        {
            Message = "This is a debug log",
            Timestamp = "[14:00:00]",
            Level = LogLevel.Debug
        });

        Logs.Add(new()
        {
            Message = "This is a benchmark log",
            Timestamp = "[14:00:01]",
            Level = LogLevel.Benchmark
        });

        Logs.Add(new()
        {
            Message = "This is an info log",
            Timestamp = "[14:00:02]",
            Level = LogLevel.Info
        });

        Logs.Add(new()
        {
            Message = "This is a warn log",
            Timestamp = "[14:00:03]",
            Level = LogLevel.Warn
        });

        Logs.Add(new()
        {
            Message = "This is an error log",
            Timestamp = "[14:01:56]",
            Level = LogLevel.Error
        });

        Logs.Add(new()
        {
            Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            Timestamp = "[14:01:56]",
            Level = LogLevel.Info
        });
    }
}