using HunterPie.Core.Analytics.Entity;
using HunterPie.Core.Client;
using HunterPie.Core.System.Windows.Native;
using System;
using System.Diagnostics;

namespace HunterPie.Core.Analytics;

public static class AnalyticsEvent
{
    public static CrashPayload FromException(
        Exception exception,
        bool isUiError
    )
    {
        var memoryStatus = new Kernel32.MemoryStatusEx();
        Kernel32.GlobalMemoryStatusEx(ref memoryStatus);
        using var process = Process.GetCurrentProcess();

        return new CrashPayload(
            Version: ClientInfo.Version.ToString(),
            GameBuild: null,
            Error: exception.GetType().ToString(),
            Stacktrace: exception.StackTrace ?? string.Empty,
            IsUiError: isUiError,
            Context: new SystemPayload(
                TotalSystemMemory: memoryStatus.ullTotalPhys,
                AllocatedMemory: process.PrivateMemorySize64,
                WindowsVersion: Environment.OSVersion.ToString()
            )
        );
    }
}