using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Report.Models;

public record CrashReportRequest(
    [property: JsonProperty("version")] string Version,
    [property: JsonProperty("game_build")] string GameBuild,
    [property: JsonProperty("exception")] string Exception,
    [property: JsonProperty("stacktrace")] string Stacktrace,
    [property: JsonProperty("is_ui_error")] bool IsUiError,
    [property: JsonProperty("context")] CrashReportContextRequest Context
);