using HunterPie.Core.Client.Localization;
using System.Xml;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Services;

public class MHRStrings
{
    public string GetMonsterNameById(int id)
    {
        string query = $"//Strings/Monsters/Rise/Monster[@Id='{id}']";
        XmlNode monster = Localization.Query(query);

        return monster?.Attributes["String"]?.Value ?? $"Unknown [id: {id}]";
    }

    public string GetStageNameById(int id)
    {
        string query = $"//Strings/Stages/Rise/Stage[@Id='{id}']";
        XmlNode monster = Localization.Query(query);

        return monster?.Attributes["String"]?.Value ?? $"Unknown [id: {id}]";
    }
}