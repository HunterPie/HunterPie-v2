using HunterPie.Core.Domain.Features.Domain;
using System;

namespace HunterPie.Core.Domain.Features;

[Obsolete("FeatureFlagManager is deprecated, use IFeatureFlagRepository with DI instead")]
public class FeatureFlagManager
{
    private IFeatureFlagRepository flagRepository;
    private static FeatureFlagManager _instance;

    internal static void Initialize(IFeatureFlagRepository repository)
    {
        if (_instance is not null)
            return;

        _instance = new()
        {
            flagRepository = repository
        };
    }

    private FeatureFlagManager() { }

    public static bool IsEnabled(string feature) => _instance.flagRepository.IsEnabled(feature);

#nullable enable
    public static IFeature? Get(string feature) => _instance.flagRepository.GetFeature(feature);
#nullable disable

    internal static void Dispose() => _instance = null;
}