using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;
using HunterPie.Features;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class FeatureFlagsInitializer : IInitializer
{
    private readonly DefaultFeatureFlags _flagsSource;

    public FeatureFlagsInitializer(
        DefaultFeatureFlags flagsSource)
    {
        _flagsSource = flagsSource;
    }

    public Task Init()
    {
        var supportedFlags = _flagsSource.Flags.Keys.ToHashSet();

        ConfigManager.Register("internal/feature-flags.json", _flagsSource.Flags);
        ConfigManager.BindConfiguration("internal/feature-flags.json", _flagsSource.Flags);
        string[] loadedFlags = _flagsSource.Flags.Keys.ToArray();

        foreach (string loadedFlag in loadedFlags)
            if (!supportedFlags.Contains(loadedFlag))
                _flagsSource.Flags.Remove(loadedFlag);

        return Task.CompletedTask;
    }
}