using HunterPie.Core.Client.Localization;
using System.Xml;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Services;

public class MHWStrings
{
    public string GetMonsterNameById(int id)
    {
        string query = $"//Strings/Monsters/World/Monster[@Id='{id}']";
        XmlNode monster = Localization.Query(query);

        return monster?.Attributes["String"].Value ?? "Unknown";
    }

    public string GetStageNameById(int id)
    {
        string query = $"//Strings/Stages/World/Stage[@Id='{id}']";
        XmlNode monster = Localization.Query(query);

        return monster?.Attributes["String"].Value ?? "Unknown";
    }
}