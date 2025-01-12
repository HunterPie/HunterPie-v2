using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Patch.Models;

public record PatchResponse(
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("description")] string Description,
    [property: JsonProperty("banner")] string Banner,
    [property: JsonProperty("link")] string Link
);