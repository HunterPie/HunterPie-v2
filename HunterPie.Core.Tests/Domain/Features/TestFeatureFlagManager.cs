using HunterPie.Core.Domain.Features;
using HunterPie.Core.Domain.Features.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace HunterPie.Core.Tests.Domain.Features;

[TestClass]
public class TestFeatureFlagManager
{
    private class MockFlagsRepository : IFeatureFlagRepository
    {
        private readonly Dictionary<string, Feature> _mockFeatures = new()
        {
            { "feature_example_mock", new Feature(true) },
            { "feature_example_mock_2", new Feature(false) },
            { "feature_example_mock_3", new Feature(true) },
            { "feature_example_mock_4", new Feature(true) },
        };

        public IFeature GetFeature(string feature) => !_mockFeatures.ContainsKey(feature) ? null : (IFeature)_mockFeatures[feature];

        public bool IsEnabled(string feature) => GetFeature(feature)?.IsEnabled ?? false;
    }

    [TestInitialize]
    public void Setup()
    {
        var mockFeatureRepo = new MockFlagsRepository();

        FeatureFlagManager.Initialize(mockFeatureRepo);
    }

    [TestCleanup]
    public void Cleanup() => FeatureFlagManager.Dispose();

    [TestMethod]
    public void FeatureFlag_WithValidFeature_ReturnsEnabledState()
    {
        Assert.IsTrue(FeatureFlagManager.IsEnabled("feature_example_mock"));
        Assert.IsFalse(FeatureFlagManager.IsEnabled("feature_example_mock_2"));
        Assert.IsTrue(FeatureFlagManager.IsEnabled("feature_example_mock_3"));
        Assert.IsTrue(FeatureFlagManager.IsEnabled("feature_example_mock_4"));
    }

    [TestMethod]
    public void FeatureFlag_WithInvalidFeature_ReturnsFalse() => Assert.IsFalse(FeatureFlagManager.IsEnabled("feature_non_existent"));

    [TestMethod]
    public void FeatureFlag_WithValidFeature_ReturnsFeature()
    {
        IFeature feature = FeatureFlagManager.Get("feature_example_mock_2");

        Assert.IsNotNull(feature);
        Assert.IsFalse(feature.IsEnabled);
    }

    [TestMethod]
    public void FeatureFlag_WithInvalidFeature_ReturnsNull() => Assert.IsNull(FeatureFlagManager.Get("feature_non_existent"));
}