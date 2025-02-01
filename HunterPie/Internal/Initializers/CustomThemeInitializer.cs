using HunterPie.Core.Client;
using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace HunterPie.Internal.Initializers;

internal class CustomThemeInitializer : IInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public Task Init()
    {
        string themePath = Path.Combine(ClientInfo.ThemesPath, ClientConfig.Config.Client.Theme);

        if (!Directory.Exists(themePath))
        {
            _logger.Error($"Failed to load theme {ClientConfig.Config.Client.Theme.Current}");
            _logger.Info($"Failed to find theme {ClientConfig.Config.Client.Theme.Current}, Changed to Default theme");
            themePath = Path.Combine(ClientInfo.ThemesPath, "Default");
            ClientConfig.Config.Client.Theme.Current = "Default";
        }

        IEnumerable<string> xamlFilesToLoad = Directory.EnumerateFiles(themePath, "*.xaml");

        foreach (string file in xamlFilesToLoad)
            TryLoadingResource(file);

        _logger.Info($"Loaded theme {ClientConfig.Config.Client.Theme.Current}");

        return Task.CompletedTask;
    }

    private void TryLoadingResource(string file)
    {
        try
        {
            using var stream = new FileStream(file, FileMode.Open, FileAccess.Read);

            var resource = (ResourceDictionary)XamlReader.Load(stream);
            Application.Current.Resources.MergedDictionaries.Add(resource);
        }
        catch (Exception err)
        {
            _logger.Error($"Failed to load custom file {file}\n{err}");
        }
    }
}