using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Report.Models;

public record CrashReportContextRequest(
    [property: JsonProperty("ram_total")] ulong RamTotal,
    [property: JsonProperty("ram_used")] long RamUsed,
    [property: JsonProperty("windows_version")] string WindowsVersion
);