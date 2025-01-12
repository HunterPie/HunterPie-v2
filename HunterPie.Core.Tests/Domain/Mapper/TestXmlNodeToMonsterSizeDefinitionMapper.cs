using HunterPie.Core.Domain.Mapper.Internal;
using HunterPie.Core.Game.Data.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;

namespace HunterPie.Core.Tests.Domain.Mapper;

[TestClass]
public class TestXmlNodeToMonsterSizeDefinitionMapper
{
    private readonly XmlNodeToMonsterSizeDefinitionMapper _target = new XmlNodeToMonsterSizeDefinitionMapper();

    [TestMethod]
    public void ShouldParseCrownsCorrectlyWithDefaultValue()
    {
        var document = new XmlDocument();
        document.LoadXml("""
            <Crowns Size="100"/>
        """);
        var expected = new MonsterSizeDefinition
        {
            Gold = 1.23f,
            Silver = 1.15f,
            Mini = 0.9f,
            Size = 100
        };
        MonsterSizeDefinition actual = _target.Map(document);

        Assert.AreEqual(expected.Gold, actual.Gold);
        Assert.AreEqual(expected.Silver, actual.Silver);
        Assert.AreEqual(expected.Mini, actual.Mini);
        Assert.AreEqual(expected.Size, actual.Size);
    }

    [TestMethod]
    public void ShouldParseCrownsCorrectlyWithExplicitValue()
    {
        var document = new XmlDocument();
        document.LoadXml("""
            <Crowns Size="100"
                    Mini="1"
                    Silver="2"
                    Gold="3"/>
        """);
        var expected = new MonsterSizeDefinition
        {
            Gold = 3,
            Silver = 2,
            Mini = 1,
            Size = 100
        };
        MonsterSizeDefinition actual = _target.Map(document);

        Assert.AreEqual(expected.Gold, actual.Gold);
        Assert.AreEqual(expected.Silver, actual.Silver);
        Assert.AreEqual(expected.Mini, actual.Mini);
        Assert.AreEqual(expected.Size, actual.Size);
    }
}