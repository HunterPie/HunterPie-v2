using HunterPie.Core.Address.Map.Internal;
using HunterPie.Core.Logger;
using System;
using System.IO;
using System.Linq;

namespace HunterPie.Core.Address.Map;

public static class AddressMap
{

    private static IAddressMapParser _parser;
    public static bool IsLoaded { get; private set; }

    public static bool Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            IsLoaded = false;
            return IsLoaded;
        }

        using FileStream file = File.OpenRead(filePath);
        using var stream = new StreamReader(file);

        _parser = new LegacyAddressMapParser(stream);

        Log.Info($"Loaded {Path.GetFileName(filePath)} successfully");

        IsLoaded = true;

        return IsLoaded;
    }

    public static bool ParseLatest(string mapsDir)
    {
        string latestMap = Directory.GetFiles(mapsDir, "*.map")
            .OrderByDescending(version => version)
            .First();

        return Parse(Path.Combine(mapsDir, latestMap));
    }

    public static T Get<T>(string key) => _parser.Get<T>(key);

    public static IntPtr GetAbsolute(string key) => (IntPtr)(_parser.Get<long>("BASE") + _parser.Get<long>(key));

    public static void Add<T>(string key, T value) => _parser.Add(key, value);

    public static int[] GetOffsets(string key) => Get<int[]>(key);
}