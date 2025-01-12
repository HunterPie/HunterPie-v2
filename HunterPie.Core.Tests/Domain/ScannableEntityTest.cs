using HunterPie.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests;

[TestClass]
public class ScannableEntityTest
{

    private struct TestDTO
    {
        public string TestValue;
    }

    private struct Test2DTO
    {
        public int TestValue;
    }

    private class TestEntity : Scannable
    {
        public string TestValue { get; private set; }
        public int TestValue2 { get; private set; }

        public TestEntity() : base()
        {
        }

        public void ScanData() => Scan();

        [ScannableMethod(typeof(TestDTO))]
        private void ScanTestData()
        {
            TestDTO dto = new()
            {
                TestValue = "DTO Value!"
            };

            Next(ref dto);
            TestValue = dto.TestValue;

        }

        [ScannableMethod(typeof(Test2DTO))]
        private void ScanTestData2()
        {
            Test2DTO dto = new() { TestValue = 20 };

            Next(ref dto);
            TestValue2 = dto.TestValue;

        }
    }

    [TestMethod]
    public void TestMiddleware()
    {
        TestEntity test = new();

        _ = test.MiddlewareFor<TestDTO>(ScannerMiddleware);
        _ = test.MiddlewareFor<Test2DTO>(ScannerMiddleware2);
        // This is called internally by the scannable entity, this is here just for testing purposes
        test.ScanData();

        Assert.AreEqual("Modified by middleware!", test.TestValue);
        Assert.AreEqual(10, test.TestValue2);
    }

    private void ScannerMiddleware(ref TestDTO data) => data.TestValue = "Modified by middleware!";

    private void ScannerMiddleware2(ref Test2DTO data) => data.TestValue = 10;
}