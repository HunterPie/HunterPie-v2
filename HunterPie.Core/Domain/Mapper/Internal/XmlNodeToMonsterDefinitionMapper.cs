using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal;

public class XmlNodeToMonsterDefinitionMapper : IMapper<XmlNode, MonsterDefinition>
{
    public MonsterDefinition Map(XmlNode data)
    {
        MonsterPartDefinition[] parts = data.SelectNodes("Parts/Part")?
            .Cast<XmlNode>()
            .Select(MapFactory.Map<XmlNode, MonsterPartDefinition>)
            .ToArray() ?? Array.Empty<MonsterPartDefinition>();

        MonsterSizeDefinition size = MapFactory.Map<XmlNode, MonsterSizeDefinition>(data);
        Element[] elements = GetMonsterElements(data);
        string[] types = GetMonsterTypes(data);

        var schema = new MonsterDefinition
        {
            Parts = parts,
            Size = size,
            Weaknesses = elements,
            Types = types
        };

        int.TryParse(
            data.Attributes?["Id"]?.Value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out schema.Id
        );

        int.TryParse(
            data.Attributes?["Capture"]?.Value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out schema.Capture
        );

        bool.TryParse(
            data.Attributes?["IsNotCapturable"]?.Value,
            out schema.IsNotCapturable
        );

        return schema;
    }

    private static Element[] GetMonsterElements(XmlNode monster)
    {
        if (monster.SelectNodes("Weaknesses/Weakness") is not { } weaknessNodes)
            return Array.Empty<Element>();

        var weaknesses = new Element[weaknessNodes.Count];
        for (int i = 0; i < weaknesses.Length; i++)
        {
            XmlNode weaknessNode = weaknessNodes[i]!;

            Enum.TryParse(
                typeof(Element),
                weaknessNode.Attributes?["Name"]?.Value,
                out object? elementObject
            );

            if (elementObject is not Element element)
                continue;

            weaknesses[i] = element;
        }

        return weaknesses;
    }

    private static string[] GetMonsterTypes(XmlNode monster)
    {
        if (monster.SelectNodes("Types/Type") is not { } typeNodes)
            return Array.Empty<string>();

        return typeNodes.Cast<XmlNode>()
            .Select(it => it.Attributes?["Name"]?.Value)
            .Where(it => !string.IsNullOrEmpty(it))
            .Cast<string>()
            .ToArray();
    }
}