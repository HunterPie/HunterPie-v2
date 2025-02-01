using HunterPie.Core.Game;
using HunterPie.Core.Settings;
using System;

namespace HunterPie.Core.Plugins;

public interface IPlugin
{
    public string Name { get; }
    public string Description { get; }
    public string Author { get; }
    public Version Version { get; }
    public ISettings Config { get; }

    internal IContext Context { get; set; }

    public void Initialize();

    public void OnLoad();
    public void OnUnload();

    public void OnEnable();
    public void OnDisable();

}