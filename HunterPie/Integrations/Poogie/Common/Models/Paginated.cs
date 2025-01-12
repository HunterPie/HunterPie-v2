using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Common.Models;

public record Paginated<T>(
    [property: JsonProperty("total_pages")] int TotalPages,
    [property: JsonProperty("current_page")] int CurrentPage,
    [property: JsonProperty("elements")] T[] Elements
);