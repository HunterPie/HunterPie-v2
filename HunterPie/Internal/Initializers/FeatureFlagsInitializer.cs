using HunterPie.Core.Client;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Domain.Features.Data;
using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Domain.Interfaces;
using HunterPie.Features;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class FeatureFlagsInitializer : IInitializer
{

    public static readonly DefaultFeatureFlags Features = new();

    public Task Init()
    {
        IFeatureFlagRepository localRepository = new LocalFeatureFlagRepository(Features.ReadOnlyFlags);

        var supportedFlags = Features.Flags.Keys.ToHashSet();

        ConfigManager.Register("internal/feature-flags.json", Features.Flags);

        string[] loadedFlags = Features.Flags.Keys.ToArray();

        foreach (string loadedFlag in loadedFlags)
            if (!supportedFlags.Contains(loadedFlag))
                Features.Flags.Remove(loadedFlag);

        FeatureFlagManager.Initialize(localRepository);

        return Task.CompletedTask;
    }
}