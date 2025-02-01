using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HunterPie.Core.Tests.Domain.Mapper;

public class TestData
{
    public float a;
    public float b;
    public float c;
}

public class TestDataAlt
{
    public int d;
    public float e;
    public float f;
}

public class TestDataToAltMapper : IMapper<TestData, TestDataAlt>
{
    public TestDataAlt Map(TestData data)
    {
        return new()
        {
            d = (int)data.a,
            e = Math.Max(data.b, data.c),
            f = data.c
        };
    }
}

[TestClass]
public class TestMapFactory
{
    [TestMethod]
    public void TestMapFactoryParser()
    {
        MapFactory.Add(new TestDataToAltMapper());

        TestData input = new()
        {
            a = 421.123f,
            b = 500.123f,
            c = 1.0f
        };

        TestDataAlt correct = new()
        {
            d = 421,
            e = 500.123f,
            f = 1.0f
        };

        TestDataAlt output = MapFactory.Map<TestData, TestDataAlt>(input);

        Assert.AreEqual(correct.d, output.d);
        Assert.AreEqual(correct.e, output.e);
        Assert.AreEqual(correct.f, output.f);
    }
}