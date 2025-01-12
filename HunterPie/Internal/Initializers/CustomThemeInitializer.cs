using HunterPie.Core.Client;
using HunterPie.Core.Logger;
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
    public Task Init()
    {
        string themePath = Path.Combine(ClientInfo.ThemesPath, ClientConfig.Config.Client.Theme);

        if (!Directory.Exists(themePath))
        {
            Log.Error("Failed to load theme {0}", ClientConfig.Config.Client.Theme.Current);
            Log.Info("Failed to find theme {0}, Changed to Default theme", ClientConfig.Config.Client.Theme.Current);
            themePath = Path.Combine(ClientInfo.ThemesPath, "Default");
            ClientConfig.Config.Client.Theme.Current = "Default";
        }

        IEnumerable<string> xamlFilesToLoad = Directory.EnumerateFiles(themePath, "*.xaml");

        foreach (string file in xamlFilesToLoad)
            TryLoadingResource(file);

        Log.Info("Loaded theme {0}", ClientConfig.Config.Client.Theme.Current);

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
            Log.Error("Failed to load custom file {0}\n{1}", file, err.ToString());
        }
    }
}