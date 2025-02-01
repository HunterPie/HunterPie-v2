using HunterPie.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace HunterPie.Core.Tests.Extensions;

[TestClass]
public class ColorExtensionsTest
{

    [TestMethod]
    public void ToHexString_ShouldParseColorToHexString()
    {
        var testCase = new
        {
            Color = Color.FromArgb(0x35, 0x33, 0xFF, 0x69),
            Expected = "#3533FF69"
        };

        string output = testCase.Color.ToHexString();

        Assert.AreEqual(testCase.Expected, output);
    }
}