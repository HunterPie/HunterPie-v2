using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Version.Models;

internal record VersionResponse(
    [property: JsonProperty("latest_version")] string LatestVersion
);