using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Game.Data.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Game.Data.Repository;

#nullable enable
public static class AbnormalityRepository
{
    private const string RISE_ABNORMALITIES_FILE = "Game/Rise/Data/AbnormalityData.xml";
    private const string WORLD_ABNORMALITIES_FILE = "Game/World/Data/AbnormalityData.xml";

    private static readonly Lazy<Dictionary<string, AbnormalityDefinition>> LazyRiseDataSource = new(() => Load(RISE_ABNORMALITIES_FILE));
    private static readonly Lazy<Dictionary<string, AbnormalityDefinition>> LazyWorldDataSource = new(() => Load(WORLD_ABNORMALITIES_FILE));

    /// <summary>
    /// Returns an abnormality with the given internal unique identifier
    /// </summary>
    /// <param name="game">The game type</param>
    /// <param name="id">The abnormality id</param>
    public static AbnormalityDefinition? FindBy(GameType game, string id)
    {
        Dictionary<string, AbnormalityDefinition> dataSource = GetDataSourceBy(game);

        return dataSource.ContainsKey(id) ? dataSource[id] : null;
    }

    /// <summary>
    /// Returns all abnormalities grouped by category
    /// </summary>
    /// <param name="game">The game type</param>
    /// <returns>Groups of abnormalities</returns>
    public static IEnumerable<IGrouping<string, AbnormalityDefinition>> FindAllAbnormalitiesBy(GameType game)
    {
        Dictionary<string, AbnormalityDefinition> dataSource = GetDataSourceBy(game);

        return dataSource.Values.GroupBy(it => it.Group);
    }

    /// <summary>
    /// Returns an array with all abnormalities that match the given category
    /// </summary>
    /// <param name="game">The game type</param>
    /// <param name="category">The category name</param>
    /// <returns>An array with the abnormalities</returns>
    public static AbnormalityDefinition[] FindAllAbnormalitiesBy(GameType game, string category)
    {
        Dictionary<string, AbnormalityDefinition> dataSource = GetDataSourceBy(game);

        return dataSource.Values.Where(it => it.Category == category)
            .ToArray();
    }

    private static Dictionary<string, AbnormalityDefinition> GetDataSourceBy(GameType game)
    {
        Lazy<Dictionary<string, AbnormalityDefinition>> dataSource = game switch
        {
            GameType.Rise => LazyRiseDataSource,
            GameType.World => LazyWorldDataSource,
            _ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
        };

        return dataSource.Value;
    }

    private static Dictionary<string, AbnormalityDefinition> Load(string file)
    {
        XmlDocument document = new();
        document.Load(ClientInfo.GetPathFor(file));

        return LoadAbnormalities(document);
    }

    private static Dictionary<string, AbnormalityDefinition> LoadAbnormalities(XmlDocument document)
    {
        XmlNodeList? abnormalityNodes = document.SelectNodes("//Abnormalities/*/Abnormality");

        if (abnormalityNodes is not { })
            return new Dictionary<string, AbnormalityDefinition>();

        int abnormalityCount = abnormalityNodes.Count;

        var dictionary = new Dictionary<string, AbnormalityDefinition>(abnormalityCount);

        foreach (XmlNode node in abnormalityNodes)
        {
            AbnormalityDefinition schema = MapFactory.Map<XmlNode, AbnormalityDefinition>(node);
            dictionary[schema.Id] = schema;
        }

        return dictionary;
    }
}