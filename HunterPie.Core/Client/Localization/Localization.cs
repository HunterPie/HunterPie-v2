using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Logger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

#nullable enable
namespace HunterPie.Core.Client.Localization;

public class Localization
{

    private readonly XmlDocument? document;
    private static Localization? _instance;

    public static Localization Instance
    {
        get
        {
            if (_instance is null)
                _instance = new();

            return _instance;
        }
    }

    private Localization()
    {
        string englishXml = Path.Combine(ClientInfo.LanguagesPath, "en-us.xml");
        document = new();
        document.Load(englishXml);

        string xmlPath = Path.Combine(ClientInfo.LanguagesPath, ClientConfig.Config.Client.Language);

        if (!File.Exists(xmlPath))
        {
            Log.Error($"Failed to find {Path.GetFileNameWithoutExtension(xmlPath)} localization");
            return;
        }

        try
        {
            // Merges selected custom localization with the default en-us one
            // to avoid missing strings
            if (ClientConfig.Config.Client.Language != "en-us.xml")
            {
                XmlDocument otherLanguage = new();
                otherLanguage.Load(xmlPath);

                XmlNodeList englishNodes = document.DocumentElement!.SelectNodes("//*")!;
                foreach (XmlNode node in otherLanguage.DocumentElement!.SelectNodes("//*")!)
                {
                    string id = node.Attributes!["Id"]?.Value!;

                    if (id is null)
                        continue;

                    string path = GetFullParentPath(node);
                    XmlNode match = document.DocumentElement.SelectSingleNode($"//{path}/*[@Id='{id}']")!;

                    if (match is null)
                        continue;

                    if (match.Attributes?["String"] is not null)
                        match.Attributes!["String"]!.Value = node.Attributes["String"]?.Value ?? match.Attributes["String"]!.Value;

                    if (match.Attributes?["Description"] is not null)
                        match.Attributes!["Description"]!.Value = node.Attributes["Description"]?.Value ?? match.Attributes["Description"]!.Value;
                }
            }
        }
        catch (Exception err)
        {
            Log.Error(err.ToString());
        }

        Log.Info($"Loaded localization {Path.GetFileNameWithoutExtension(xmlPath)}");
    }

    private static string GetFullParentPath(XmlNode node, string path = "")
    {
        return node.ParentNode?.Name == null || node.ParentNode.Name == "#document"
            ? path
            : GetFullParentPath(node.ParentNode, $"{node.ParentNode.Name}/{path}");
    }

    public static XmlNode? Query(string query) => Instance.document?.SelectSingleNode(query);
    public static XmlNodeList? QueryMany(string query) => Instance.document?.SelectNodes(query);
    public static string QueryString(string query) => Instance.document?.SelectSingleNode(query)?.Attributes?["String"]?.Value ?? query;
    public static string QueryDescription(string query) => Query(query)?.Attributes?["Description"]?.Value ?? query;

    public static (string, string) Resolve(string path)
    {
        XmlNode? node = Query(path);
        XmlAttributeCollection? attributes = node?.Attributes;

        return (attributes?["String"]?.Value ?? path, attributes?["Description"]?.Value ?? path);
    }

    public static string GetEnumString<T>(T enumValue)
    {
        MemberInfo memberInfo = enumValue.GetType()
                                         .GetMember(enumValue.ToString()!)
                                         .First();

        LocalizationAttribute? attribute = memberInfo.GetCustomAttribute<LocalizationAttribute>();

        return attribute is null ? enumValue.ToString()! : QueryString(attribute.XPath)!;
    }

    public static string? GetQuestNameBy(GameType game, int questId)
    {
        XmlNode? translation = Query($"//Strings/Quests/{game}/Quest[@Id='{questId}']");

        return translation?.Attributes?["String"]?.Value;
    }
}
#nullable restore