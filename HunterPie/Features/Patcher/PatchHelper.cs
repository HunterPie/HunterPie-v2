using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Memory;
using System;
using System.Linq;

namespace HunterPie.Features.Patcher;

internal static class PatchHelper
{
    /// <inheritdoc cref="ContentEquals(IMemory,long,string)"/>
    /// <param name="memoryLocationName">the name of an Address existing in <see cref="AddressMap"/>.</param>
    public static bool ContentEquals(this IMemory memory, string memoryLocationName, string expectedDataName)
    {
        long memoryLocation = AddressMap.GetAbsolute(memoryLocationName);
        return ContentEquals(memory, memoryLocation, expectedDataName);
    }

    /// <summary>
    /// Checks whether the process memory sequence starting from the specified memory address
    /// has the same data.
    /// </summary>
    /// <param name="memory">the memory to inspect.</param>
    /// <param name="memoryLocation">starting memory address.</param>
    /// <param name="expectedDataName">the name of a byte sequence (Instruction) existing in <see cref="AddressMap"/>.</param>
    public static bool ContentEquals(this IMemory memory, long memoryLocation, string expectedDataName)
    {
        // TODO AddressMap.Get should support returning byte[] directly.
        byte[] expectedData = AddressMap.Get<int[]>(expectedDataName)
            .Select(b => (byte)b)
            .ToArray();
        if (expectedData.Length is < 1 or > 32)
        {
            // This is only a safeguard against blatant errors in the address map.
            throw new InvalidOperationException($"Unexpected data length: {expectedData.Length}.");
        }

        byte[] actualData = memory.Read<byte>(memoryLocation, (uint)expectedData.Length);
        return actualData.SequenceEqual(expectedData);
    }
}