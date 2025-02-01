using HunterPie.Core.Observability.Logging;
using System;
using System.Linq;
using AddressMapType = HunterPie.Core.Address.Map.Internal.AddressMapKeyWords.AddressMapType;

namespace HunterPie.Core.Address.Map.Internal;

internal static class AddressMapParserExtensions
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private static int[] ParseStringToVecInt32(string stringified)
    {
        return stringified.Split(",")
                .Select(element => Convert.ToInt32(element.Trim(), 16))
                .ToArray();
    }

    public static void AddValueByType(this IAddressMapParser self, AddressMapType type, string key, string value)
    {
        void TryAdd<T>(string key, T value)
        {
            try
            {
                self.Add(key, value);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        switch (type)
        {
            case AddressMapType.Pointer:
                TryAdd(key, (nint)Convert.ToInt64(value, 16));
                break;

            case AddressMapType.Offsets:
                TryAdd(key, ParseStringToVecInt32(value));
                break;

            default:
                break;

        }
    }
}