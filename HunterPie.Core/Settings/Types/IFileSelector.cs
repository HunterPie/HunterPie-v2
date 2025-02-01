using Newtonsoft.Json;
using System.Collections.Generic;

namespace HunterPie.Core.Settings.Types;

public interface IFileSelector
{
    [JsonProperty]
    public string Current { get; set; }

    public IEnumerable<string> GetElements();
}