using HunterPie.Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace HunterPie.Core.Tests.Utils;

[TestClass]
public class MarshalHelperTest
{
    [StructLayout(LayoutKind.Sequential, Size = 19)]
    public struct TestStruct
    {
        public long Unk0;
        public long Unk1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Unk2;
    }

    [TestMethod]
    public void TestStructureToBuffer()
    {
        byte[] correct = new byte[19]
        {
            0xEF, 0xBE, 0xAD, 0xDE, 0x00, 0x00, 0x00, 0x00,
            0xEE, 0xFF, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x75, 0x77, 0x75
        };
        var test = new TestStruct()
        {
            Unk0 = 0xDEADBEEF,
            Unk1 = 0xC0FFEE,
            Unk2 = new byte[] { 0x75, 0x77, 0x75 }
        };
        TestStruct[] array = new[] { test };

        byte[] buffer = MarshalHelper.StructureToBuffer(array);

        for (int i = 0; i < buffer.Length; i++)
            Assert.AreEqual(correct[i], buffer[i]);
    }

    [TestMethod]
    public void TestBufferToStructure()
    {
        byte[] buffer = new byte[19]
        {
            0xEF, 0xBE, 0xAD, 0xDE, 0x00, 0x00, 0x00, 0x00,
            0xEE, 0xFF, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x75, 0x77, 0x75
        };

        // Move buffer to an unmanaged area
        IntPtr ptr = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, ptr, buffer.Length);

        TestStruct @struct = MarshalHelper.BufferToStructures<TestStruct>(ptr, 1)
                                          .FirstOrDefault();

        Assert.IsNotNull(@struct);
        Assert.AreEqual(@struct.Unk0, 0xDEADBEEF);
        Assert.AreEqual(@struct.Unk1, 0xC0FFEE);
        Assert.AreEqual(Encoding.ASCII.GetString(@struct.Unk2), "uwu");
    }
}