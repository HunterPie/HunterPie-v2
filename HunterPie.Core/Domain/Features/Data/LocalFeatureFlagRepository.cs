using HunterPie.Core.Domain.Features.Domain;
using System.Collections.Generic;

namespace HunterPie.Core.Domain.Features.Data;

internal class LocalFeatureFlagRepository : IFeatureFlagRepository
{

    public readonly IReadOnlyDictionary<string, IFeature> _features;

    public LocalFeatureFlagRepository(IReadOnlyDictionary<string, IFeature> source)
    {
        _features = source;
    }

    public IFeature GetFeature(string feature) => !_features.ContainsKey(feature) ? null : _features[feature];

    public bool IsEnabled(string feature) => _features.ContainsKey(feature) && (bool)_features[feature].IsEnabled;
}