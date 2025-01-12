namespace HunterPie.Core.Analytics.Entity;

public record SystemPayload(
    ulong TotalSystemMemory,
    long AllocatedMemory,
    string WindowsVersion
);