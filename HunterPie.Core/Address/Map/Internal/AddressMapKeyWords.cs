using System.Collections.Generic;

namespace HunterPie.Core.Address.Map.Internal;

internal static class AddressMapKeyWords
{
    public enum AddressMapType
    {
        Long,
        VecInt32,
        Unknown
    }

    private static readonly Dictionary<string, AddressMapType> types = new()
    {
        { "Address", AddressMapType.Long },
        { "Offset", AddressMapType.VecInt32 },
        { "Instruction", AddressMapType.VecInt32 },
    };

    public static bool IsKeyWord(string word) => types.ContainsKey(word);
    public static AddressMapType GetType(string word) => IsKeyWord(word) ? types[word] : AddressMapType.Unknown;
}