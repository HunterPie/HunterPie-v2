using HunterPie.Core.Address.Map.Internal;
using HunterPie.Core.Observability.Logging;
using System.IO;
using System.Linq;

namespace HunterPie.Core.Address.Map;

public static class AddressMap
{
    private static readonly ILogger Logger = LoggerFactory.Create();
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

        Logger.Info($"Loaded {Path.GetFileName(filePath)} successfully");

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

    public static nint GetAbsolute(string key) => _parser.Get<nint>("BASE") + _parser.Get<nint>(key);

    public static void Add<T>(string key, T value) => _parser.Add(key, value);

    public static int[] GetOffsets(string key) => Get<int[]>(key);
}