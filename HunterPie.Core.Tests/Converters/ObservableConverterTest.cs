using HunterPie.Core.Architecture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace HunterPie.Core.Tests.Converters;

[TestClass]
public class ObservableConverterTest
{
    public class TestClass
    {
        public Observable<bool> Prop1 { get; set; } = true;
        public Observable<bool> Prop2 { get; set; } = false;
        public Observable<int> Prop3 { get; set; } = 10;
    }

    [TestMethod]
    public void TestSerialize()
    {
        TestClass toSerialize = new();
        string result = JsonConvert.SerializeObject(toSerialize);
        string correct = "{\"Prop1\":true,\"Prop2\":false,\"Prop3\":10}";

        Assert.AreEqual(correct, result);
    }

    [TestMethod]
    public void TestDeserialize()
    {
        string serialized = "{\"Prop1\":false,\"Prop2\":true,\"Prop3\":20}";
        TestClass result = JsonConvert.DeserializeObject<TestClass>(serialized);

        Assert.IsFalse(result.Prop1);
        Assert.IsTrue(result.Prop2);
        Assert.IsTrue(20 == result.Prop3);
    }
}