using HunterPie.Core.Domain.Features.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests.Domain.Features;

[TestClass]
public class TestFeature
{
    private class MockFeature : Feature
    {

        public MockFeature(bool defaultValue) : base(defaultValue) { }

        // TODO: Find a way to test if a method was called in MSTest
        public string TestCalls = "Nothing";
        public int CallCount = 0;

        protected override void OnEnable()
        {
            base.OnEnable();

            TestCalls = "OnEnable";
            CallCount++;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            TestCalls = "OnDisable";
            CallCount++;
        }
    }

    [TestMethod]
    public void Feature_WhenEnabled_ShouldExecuteOnEnable()
    {
        var feat = new MockFeature(false);

        feat.IsEnabled.Value = true;

        Assert.AreEqual(feat.TestCalls, "OnEnable");
        Assert.AreEqual(feat.CallCount, 1);
    }

    [TestMethod]
    public void Feature_WhenDisabled_ShouldExecuteOnDisable()
    {
        var feat = new MockFeature(true);

        feat.IsEnabled.Value = false;

        Assert.AreEqual(feat.TestCalls, "OnDisable");
        Assert.AreEqual(feat.CallCount, 1);
    }
}