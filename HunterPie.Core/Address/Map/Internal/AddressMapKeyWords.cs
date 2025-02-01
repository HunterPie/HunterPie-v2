using System.Collections.Generic;

namespace HunterPie.Core.Address.Map.Internal;

internal static class AddressMapKeyWords
{
    public enum AddressMapType
    {
        Pointer,
        Offsets,
        Unknown
    }

    private static readonly Dictionary<string, AddressMapType> types = new()
    {
        { "Address", AddressMapType.Pointer },
        { "Offset", AddressMapType.Offsets },
        { "Instruction", AddressMapType.Offsets },
    };

    public static bool IsKeyWord(string word) => types.ContainsKey(word);
    public static AddressMapType GetType(string word) => IsKeyWord(word) ? types[word] : AddressMapType.Unknown;
}