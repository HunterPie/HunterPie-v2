using System;

namespace HunterPie.Core.Client.Events;

public class ConfigSaveEventArgs : EventArgs
{

    public DateTime SyncedAt { get; }
    public string Path { get; }

    public ConfigSaveEventArgs(string path)
    {
        Path = path;
        SyncedAt = DateTime.Now;
    }
}