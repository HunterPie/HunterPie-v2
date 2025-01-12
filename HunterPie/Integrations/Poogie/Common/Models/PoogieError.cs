using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Common.Models;

internal record PoogieError(
    [property: JsonProperty("code", Required = Required.Always)] PoogieErrorCode Code,
    [property: JsonProperty("error", Required = Required.Always)] string Error
)
{
    public static PoogieError Default() => new(
        Code: PoogieErrorCode.UNKNOWN_ERROR,
        Error: "Unknown"
    );
}