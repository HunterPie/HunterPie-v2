using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Converters;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HunterPie.Core.Settings.Types;

public class AbnormalityTrays
{
    [JsonConverter(typeof(ObservableCollectionConverter<AbnormalityWidgetConfig>))]
    public ObservableCollection<AbnormalityWidgetConfig> Trays { get; set; } = new();

    public AbnormalityWidgetConfig this[int index] => Trays[index];

}