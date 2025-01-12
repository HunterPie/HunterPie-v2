using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data.Definitions;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal;

public class XmlNodeToAilmentDefinitionMapper : IMapper<XmlNode, AilmentDefinition>
{
    public AilmentDefinition Map(XmlNode data)
    {
        string? id = data.Attributes?["Id"]?.Value;
        string? stringId = data.Attributes?["String"]?.Value;

        var definition = new AilmentDefinition { String = stringId! };

        int.TryParse(id, out definition.Id);

        definition.IsUnknown = definition.Id < 0 || !definition.String.StartsWith("AILMENT");

        return definition;
    }
}