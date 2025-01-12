using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Game.Data.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Game.Data.Repository;

public static class MonsterAilmentRepository
{
    private const string RISE_DATA_FILE = "Game/Rise/Data/MonsterData.xml";
    private const string WORLD_DATA_FILE = "Game/World/Data/MonsterData.xml";

    private static readonly Lazy<Dictionary<int, AilmentDefinition>> LazyRiseAilmentsDataSource = new(() => LoadAilments(RISE_DATA_FILE));
    private static readonly Lazy<Dictionary<int, AilmentDefinition>> LazyWorldAilmentsDataSource = new(() => LoadAilments(WORLD_DATA_FILE));

    public static readonly AilmentDefinition Enrage = new AilmentDefinition { Id = 99, String = "STATUS_ENRAGE" };

    /// <summary>
    /// Finds the ailment definition based on game and id
    /// </summary>
    /// <param name="game">Game</param>
    /// <param name="id">Ailment id</param>
    /// <returns>Ailment definition if found, else null</returns>
    public static AilmentDefinition? FindBy(GameType game, int id)
    {
        Dictionary<int, AilmentDefinition> dataSource = GetAilmentsDataSourceBy(game);

        return dataSource.ContainsKey(id) ? dataSource[id] : null;
    }

    /// <summary>
    /// Returns ailment definitions based on game
    /// </summary>
    /// <param name="game">Game</param>
    /// <returns>Array of ailments</returns>
    public static AilmentDefinition[] FindAllBy(GameType game)
    {
        return GetAilmentsDataSourceBy(game).Values.ToArray();
    }

    private static Dictionary<int, AilmentDefinition> GetAilmentsDataSourceBy(GameType game)
    {
        Lazy<Dictionary<int, AilmentDefinition>> dataSource = game switch
        {
            GameType.Rise => LazyRiseAilmentsDataSource,
            GameType.World => LazyWorldAilmentsDataSource,
            _ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
        };

        return dataSource.Value;
    }

    private static Dictionary<int, AilmentDefinition> LoadAilments(string file)
    {
        var document = new XmlDocument();
        document.Load(ClientInfo.GetPathFor(file));
        return LoadAilmentsFromDocument(document);
    }

    private static Dictionary<int, AilmentDefinition> LoadAilmentsFromDocument(XmlDocument document)
    {
        var definitions = document.SelectNodes("//GameData/Ailments/Ailment")!
            .Cast<XmlNode>()
            .Select(MapFactory.Map<XmlNode, AilmentDefinition>)
            .ToDictionary(it => it.Id);

        definitions.Add(Enrage.Id, Enrage);

        return definitions;
    }
}