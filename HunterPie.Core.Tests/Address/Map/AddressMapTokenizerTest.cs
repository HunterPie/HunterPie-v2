using HunterPie.Core.Address.Map.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace HunterPie.Core.Tests.Address.Map;

[TestClass]
public class AddressMapTokenizerTest
{
    [TestMethod]
    public void ConsumeUntilCharTest()
    {
        string testCase = "# this is a commentary, and I'm trying to make it as long as possible to also test the speed\nAddress TEST_OFFSET 0x123456\n";

        using var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(testCase)));

        string result = AddressMapTokenizer.ConsumeUntilChar(stream, '\n');

        Assert.AreEqual(
            "# this is a commentary, and I'm trying to make it as long as possible to also test the speed",
            result,
            "Result was different from the expected."
        );
    }
}