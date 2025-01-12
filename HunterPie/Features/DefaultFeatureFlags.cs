using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Internal.Logger;
using System.Collections.Generic;

namespace HunterPie.Features;

internal class DefaultFeatureFlags
{
    internal readonly Dictionary<string, IFeature> Flags = new()
    {
        { FeatureFlags.FEATURE_NATIVE_LOGGER, new NativeLoggerFeature() },
        { FeatureFlags.FEATURE_METRICS_WIDGET, new Feature() },
        { FeatureFlags.FEATURE_ADVANCED_DEV, new Feature() },
        { FeatureFlags.FEATURE_REDIRECT_POOGIE, new Feature() },
        { FeatureFlags.FEATURE_IN_APP_NOTIFICATIONS, new Feature() },
    };

    public IReadOnlyDictionary<string, IFeature> ReadOnlyFlags => Flags;
}