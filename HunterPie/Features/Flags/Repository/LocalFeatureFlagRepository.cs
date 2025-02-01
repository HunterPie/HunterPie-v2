using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Core.Domain.Features.Repository;
using System.Collections.Generic;

namespace HunterPie.Features.Flags.Repository;

internal class LocalFeatureFlagRepository : IFeatureFlagRepository
{
    private readonly IReadOnlyDictionary<string, IFeature> _features;

    public LocalFeatureFlagRepository(IReadOnlyDictionary<string, IFeature> source)
    {
        _features = source;
    }

    public IFeature? GetFeature(string feature) => !_features.ContainsKey(feature) ? null : _features[feature];

    public bool IsEnabled(string feature) => _features.ContainsKey(feature) && (bool)_features[feature].IsEnabled;
}