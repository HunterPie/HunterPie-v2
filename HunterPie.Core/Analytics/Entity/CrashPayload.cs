namespace HunterPie.Core.Analytics.Entity;

public record CrashPayload(
    string Version,
    string? GameBuild,
    string Error,
    string Stacktrace,
    bool IsUiError,
    SystemPayload Context
) : IAnalyticsEvent;