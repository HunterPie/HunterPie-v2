using HunterPie.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests.Extensions;

[TestClass]
public class LongExtensionsTest
{
    [TestMethod]
    public void FormatBytes_ShouldFormatBytesToShortRepresentation()
    {
        var testCases = new
        {
            Expected = new string[] { $"{1:0.0}B", $"{1:0.0}KB", $"{1:0.0}MB", $"{1:0.0}GB" },
            Values = new long[] { 1L, 1024L, 1_048_576L, 1_073_741_824L },
        };

        for (int i = 0; i < testCases.Values.Length; i++)
        {
            string expected = testCases.Expected[i];
            long value = testCases.Values[i];
            string actual = value.FormatBytes();

            Assert.AreEqual(expected, actual);
        }
    }
}