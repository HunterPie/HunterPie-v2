using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal;

internal class XmlNodeToAbnormalityDefinitionMapper : IMapper<XmlNode, AbnormalityDefinition>
{

    private const string UNKNOWN_ABNORMALITY_NAME = "ABNORMALITY_UNKNOWN";
    private const string ICON_MISSING = "ICON_MISSING";

    private readonly IAbnormalityFlagTypeParser[] _flagParsers;

    public XmlNodeToAbnormalityDefinitionMapper(params IAbnormalityFlagTypeParser[] flagParsers)
    {
        _flagParsers = flagParsers;
    }

    public AbnormalityDefinition Map(XmlNode data)
    {
        string id = data.Attributes!["Id"]!.Value;
        string name = data.Attributes["Name"]?.Value ?? UNKNOWN_ABNORMALITY_NAME;
        string icon = data.Attributes["Icon"]?.Value ?? ICON_MISSING;
        string offset = data.Attributes["Offset"]?.Value ?? id;
        string dependsOn = data.Attributes["DependsOn"]?.Value ?? "0";
        string withValue = data.Attributes["WithValue"]?.Value ?? "0";
        string group = data.ParentNode!.Name;
        string category = data.Attributes["Category"]?.Value ?? group;
        string isBuildup = data.Attributes["IsBuildup"]?.Value ?? "False";
        string maxBuildup = data.Attributes["MaxBuildup"]?.Value ?? "0";
        string isInfinite = data.Attributes["IsInfinite"]?.Value ?? "False";
        string maxTimer = data.Attributes["MaxTimer"]?.Value ?? "0";
        string flagType = data.Attributes["FlagType"]?.Value ?? "None";
        string flag = data.Attributes["Flag"]?.Value ?? "None";

        var schema = new AbnormalityDefinition
        {
            Id = BuildId(id, group),
            Name = name,
            Icon = icon,
            Category = category,
            Group = group
        };

        int.TryParse(offset, NumberStyles.HexNumber, null, out schema.Offset);
        int.TryParse(dependsOn, NumberStyles.HexNumber, null, out schema.DependsOn);
        int.TryParse(withValue, out schema.WithValue);
        bool.TryParse(isBuildup, out schema.IsBuildup);
        int.TryParse(maxBuildup, out schema.MaxBuildup);
        bool.TryParse(isInfinite, out schema.IsInfinite);
        int.TryParse(maxTimer, out schema.MaxTimer);
        Enum.TryParse(flagType, out schema.FlagType);

        schema.Flag = _flagParsers
            .Select(it => it.Parse(schema.FlagType, flag))
            .FirstOrDefault(it => it != null);

        return schema;
    }

    private static string BuildId(string id, string group)
    {
        return id.StartsWith("ABN_")
            ? id
            : $"{group}_{id}";
    }
}