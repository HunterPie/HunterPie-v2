using HunterPie.Core.Client.Events;
using HunterPie.Core.Client.Observer;
using HunterPie.Core.Extensions;
using HunterPie.Core.Json;
using HunterPie.Core.Observability.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace HunterPie.Core.Client;

public class ConfigManager
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private static readonly FileSystemWatcher _fileSystemWatcher = new()
    {
        Path = ClientInfo.ClientPath,
        Filter = "*.json",
        NotifyFilter = NotifyFilters.LastWrite,
        EnableRaisingEvents = true,
        IncludeSubdirectories = true
    };

    private static readonly Dictionary<string, long> _lastWrites = new();
    private const long MinTicks = 100 * TimeSpan.TicksPerMillisecond;
    private static readonly Dictionary<string, object> _settings = new();

    public static event EventHandler<ConfigSaveEventArgs> OnSync;

    public static IReadOnlyDictionary<string, object> Settings => _settings;

    /// <summary>
    /// Registers a new configuration file for HunterPie to keep track of.
    /// Configuration files MUST be in the json format and also have their file names
    /// end in .json
    /// </summary>
    /// <param name="path">Absolute or relative path for the configuration file</param>
    /// <param name="default">Base class for the config to be serialized to</param>
    public static void Register(string path, object @default)
    {
        path = ConfigHelper.GetFullPath(path);

        if (!Directory.Exists(path))
            _ = Directory.CreateDirectory(Path.GetDirectoryName(path));

        if (_settings.ContainsKey(path))
            return;

        _settings[path] = @default;
        Reload(path);
    }

    internal static void Initialize()
    {
        Action<string> reloadSetting = Reload;
        Action<string> debounceReload = reloadSetting.Debounce(200);

        _fileSystemWatcher.Changed += (_, args) =>
        {
            if (!Settings.ContainsKey(args.FullPath))
                return;

            long lastWrite = File.GetLastWriteTime(args.FullPath).Ticks;

            if (!_lastWrites.ContainsKey(args.FullPath)
                || (lastWrite - _lastWrites[args.FullPath] > MinTicks))
            {
                _lastWrites[args.FullPath] = lastWrite;

                debounceReload(args.FullPath);
            }
        };
    }

    public static void Reload(string path)
    {
        path = ConfigHelper.GetFullPath(path);

        if (!Settings.ContainsKey(path))
        {
            Logger.Warning($"'{path}' not registered in ConfigManager.");
            return;
        }

        if (!File.Exists(path))
        {
            string fileName = Path.GetFileName(path);
            Logger.Error($"'{fileName}' not registered in ConfigManager.");

            WriteSettings(path);
        }

        ReadSettings(path);
        OnSync?.Invoke(null, new(path));
    }

    public static void Save(string path)
    {
        path = ConfigHelper.GetFullPath(path);

        if (!Settings.ContainsKey(path))
        {
            Logger.Warning($"'{path}' not registered in ConfigManager.");
            return;
        }

        WriteSettings(path);
    }

    public static void SaveAll()
    {
        foreach (string config in Settings.Keys)
            Save(config);
    }

    private static void ReadSettings(string path)
    {
        lock (_settings[path])
            try
            {
                string str = ConfigHelper.ReadObject(path);

                JsonProvider.Populate(str, _settings[path]);
            }
            catch (Exception err)
            {
                Logger.Error(err.ToString());
            }
    }

    private static void WriteSettings(string path)
    {
        lock (_settings[path])
            ConfigHelper.WriteObject(path, _settings[path]);
    }

    public static void BindConfiguration(string path, object data)
    {
        ConfigurationBinder.Bind(data, () => Save(path));
    }

    [Obsolete]
    public static void BindAndSaveOnChanges(string path, object data)
    {
        void HandleSaveConfig(object source, EventArgs args)
        {
            Save(path);
        }

        if (data is null)
            return;

        Type type = data.GetType();
        foreach (PropertyInfo propertyInfo in type.GetProperties())
            if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
            {
                var array = (IEnumerable)propertyInfo.GetValue(data);

                foreach (object item in array)
                    BindAndSaveOnChanges(path, item);

                if (array is INotifyCollectionChanged collection)
                    collection.CollectionChanged += (_, e) =>
                    {
                        if (e.OldItems is { })
                            foreach (object item in e.OldItems)
                                if (item is INotifyPropertyChanged observable)
                                    observable.PropertyChanged -= HandleSaveConfig;

                        if (e.NewItems is { })
                            foreach (object item in e.NewItems)
                                if (item is INotifyPropertyChanged observable)
                                    observable.PropertyChanged += HandleSaveConfig;

                        Save(path);
                    };

                if (array is INotifyPropertyChanged observableCollection)
                    observableCollection.PropertyChanged += HandleSaveConfig;
            }
            else
            {
                if (propertyInfo.PropertyType.IsPrimitive)
                    continue;

                try
                {
                    object value = propertyInfo.GetValue(data);

                    if (value is INotifyPropertyChanged bindable)
                    {
                        bindable.PropertyChanged += HandleSaveConfig;
                        continue;
                    }

                    BindAndSaveOnChanges(path, value);

                }
                catch
                {
                    continue;
                }
            }
    }
}