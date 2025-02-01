using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;
using HunterPie.Core.Observability.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace HunterPie.Features.Languages.Repository;

internal class LocalizationRepository : ILocalizationRepository
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private readonly Lazy<XmlDocument> _document = new(LocalizationDocumentFactory, LazyThreadSafetyMode.ExecutionAndPublication);

    public LocalizationData FindBy(string path)
    {
        XmlAttributeCollection? attributes = _document.Value.SelectSingleNode(path)?.Attributes;

        if (attributes is not { } || attributes["String"]?.Value is not { } stringValue)
            return CreateDefault(path);

        return new LocalizationData(
            String: stringValue,
            Description: attributes["Description"]?.Value ?? path
        );
    }

    public string FindStringBy(string path)
    {
        XmlAttributeCollection? attributes = _document.Value.SelectSingleNode(path)?.Attributes;

        if (attributes is not { } || attributes["String"]?.Value is not { } stringValue)
            return path;

        return stringValue;
    }

    public LocalizationData FindByEnum<T>(T value) where T : Enum
    {
        MemberInfo? memberInfo = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault();

        if (memberInfo is not { })
            return CreateDefault(value.ToString());

        LocalizationAttribute? attribute = memberInfo.GetCustomAttribute<LocalizationAttribute>();

        return attribute switch
        {
            { } => FindBy(attribute.XPath),
            _ => CreateDefault(value.ToString())
        };
    }

    public IScopedLocalizationRepository WithScope(string scope) =>
        new ScopedLocalizationRepository(
            scopePath: scope,
            localizationRepository: this
        );

    #region Loading localization document

    private static LocalizationData CreateDefault(string path) => new LocalizationData(
        String: path,
        Description: path
    );

    private static XmlDocument LocalizationDocumentFactory()
    {
        string defaultDocument = Path.Combine(ClientInfo.LanguagesPath, "en-us.xml");

        if (!File.Exists(defaultDocument))
            throw new FileNotFoundException("Default localization file not found");

        var document = new XmlDocument();
        document.Load(defaultDocument);

        string selectedLanguageDocument = Path.Combine(ClientInfo.LanguagesPath, ClientConfig.Config.Client.Language);

        if (!File.Exists(selectedLanguageDocument))
            throw new FileNotFoundException(
                $"Failed to find localization {Path.GetFileNameWithoutExtension(selectedLanguageDocument)}");

        if (defaultDocument == selectedLanguageDocument)
        {
            Logger.Info("Loaded default language successfully");
            return document;
        }

        XmlDocument otherLanguage = new();
        otherLanguage.Load(selectedLanguageDocument);

        XmlDocument finalDocument = MergeDocuments(
            source: otherLanguage,
            target: document
        );

        Logger.Info($"Loaded localization {Path.GetFileNameWithoutExtension(defaultDocument)} successfully");

        return finalDocument;
    }

    private static XmlDocument MergeDocuments(XmlDocument source, XmlDocument target)
    {
        if (target.DocumentElement?.SelectNodes("//*") is not { } defaultNodes)
            return target;

        foreach (XmlNode node in defaultNodes)
        {
            string? id = node.Attributes?["Id"]?.Value;

            if (id is null)
                continue;

            string path = GetFullParentPath(node);
            XmlNode? match = source.DocumentElement?.SelectSingleNode($"//{path}/*[@Id='{id}']");

            if (match?.Attributes?["String"] is { } stringAttribute)
                node.Attributes!["String"]!.Value = stringAttribute.Value;

            if (match?.Attributes?["Description"] is { } descriptionAttribute)
                node.Attributes!["Description"]!.Value = descriptionAttribute.Value;
        }

        return target;
    }

    private static string GetFullParentPath(XmlNode node, string path = "")
    {
        while (true)
        {
            if (node.ParentNode?.Name is null || node.ParentNode.Name == "#document")
                return path;

            node = node.ParentNode;
            path = $"{node.Name}/{path}";
        }
    }

    #endregion
}