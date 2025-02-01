namespace HunterPie.Features.Analytics.Entity;

public record SystemPayload(
    ulong TotalSystemMemory,
    long AllocatedMemory,
    string WindowsVersion
);