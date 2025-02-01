using HunterPie.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests.Extensions;

[TestClass]
public class UIntExtensionsTest
{

    [TestMethod]
    public void ApproximateHigh_ShouldApproximateToTheNearestHigherNumber()
    {
        var testCase = new
        {
            Value = 171483u,
            PossibleValues = new uint[] { 54000u, 72000u, 126000u, 180000u },
            Expected = 180000u
        };

        uint output = testCase.Value.ApproximateHigh(testCase.PossibleValues);

        Assert.AreEqual(testCase.Expected, output);
    }

    [TestMethod]
    public void ApproximateHigh_ShouldReturnItselfWhenValueIsAPossibleValue()
    {
        var testCase = new
        {
            Value = 126000u,
            PossibleValues = new uint[] { 54000u, 72000u, 126000u, 180000u },
            Expected = 126000u
        };

        uint output = testCase.Value.ApproximateHigh(testCase.PossibleValues);

        Assert.AreEqual(testCase.Expected, output);
    }
}