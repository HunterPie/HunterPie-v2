using HunterPie.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests.Extensions;

[TestClass]
public class IntExtensionTest
{
    [TestMethod]
    public void ToPetId_ShouldConvertIntToAPetId()
    {
        var testCase = new
        {
            Value = 3,
            Expected = 8
        };

        int actual = testCase.Value.ToPetId();

        Assert.AreEqual(testCase.Expected, actual);
    }
}