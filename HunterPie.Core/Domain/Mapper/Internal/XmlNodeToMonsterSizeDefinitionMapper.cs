using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data.Definitions;
using System.Globalization;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal;

public class XmlNodeToMonsterSizeDefinitionMapper : IMapper<XmlNode, MonsterSizeDefinition>
{
    private const float MINI_CROWN = 0.9f;
    private const float SILVER_CROWN = 1.15f;
    private const float GOLD_CROWN = 1.23f;

    public MonsterSizeDefinition Map(XmlNode data)
    {
        var schema = new MonsterSizeDefinition
        {
            Mini = MINI_CROWN,
            Silver = SILVER_CROWN,
            Gold = GOLD_CROWN
        };

        XmlNode? crownsNode = data.SelectSingleNode("Crowns");

        float.TryParse(
            crownsNode?.Attributes?["Size"]?.Value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out schema.Size
        );

        if (crownsNode?.Attributes?["Mini"]?.Value is { } mini)
            float.TryParse(
                mini,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out schema.Mini
            );

        if (crownsNode?.Attributes?["Silver"]?.Value is { } silver)
            float.TryParse(
                silver,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out schema.Silver
            );

        if (crownsNode?.Attributes?["Gold"]?.Value is { } gold)
            float.TryParse(
                gold,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out schema.Gold
            );

        return schema;
    }
}