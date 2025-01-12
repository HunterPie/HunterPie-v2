using HunterPie.Core.Address.Map;
using HunterPie.Core.Address.Map.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HunterPie.Core.Tests.Address.Map;

[TestClass]
public class AddressMapTest
{
    private const string mapExample =
        "# Addresses\n" +
        "Address BASE 0x140000000\r\n #hello world!\n" +
        "Address LEVEL_OFFSET 0x04ECB810\n" +
        "Address ZONE_OFFSET 0x04EC7030\n" +
        "# Offsets\n" +
        "Offset LevelOffsets 0xA8,0x90\n" +
        "Offset ZoneOffsets 0xAED0";
    private readonly Dictionary<Type, Dictionary<string, object>> correctResult = new()
    {
        {
            typeof(long), new Dictionary<string, object>()
            {
                { "BASE", 0x140000000L },
                { "LEVEL_OFFSET", 0x04ECB810L },
                { "ZONE_OFFSET", 0x04EC7030L }
            }
        },
        {
            typeof(int[]), new Dictionary<string, object>()
            {
                { "LevelOffsets", new [] { 0xA8, 0x90 } },
                { "ZoneOffsets", new [] { 0xAED0 } }
            }
        }
    };

    [TestMethod]
    public void TestLegacyAddressMapParser()
    {
        using var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(mapExample)));

        IAddressMapParser result = new LegacyAddressMapParser(stream);

        // Assert parsed result
        foreach (Type type in result.Types)
        {
            bool exists = correctResult.ContainsKey(type);
            Assert.IsTrue(exists);

            if (exists)
            {
                foreach (KeyValuePair<string, object> map in result.Items[type])
                {
                    bool keyExists = correctResult[type].ContainsKey(map.Key);
                    Assert.IsTrue(keyExists);

                    if (keyExists)
                    {
                        if (map.Value is not Array)
                            Assert.AreEqual(correctResult[type][map.Key], map.Value);
                    }
                }
            }
        }
    }
}