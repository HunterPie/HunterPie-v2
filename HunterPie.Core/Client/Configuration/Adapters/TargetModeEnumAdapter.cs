using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Adapters;
using System;

namespace HunterPie.Core.Client.Configuration.Adapters;

internal class TargetModeEnumAdapter : IEnumAdapter
{
    private static readonly Lazy<object[]> RiseValues = new(() => new object[] { TargetModeType.LockOn, TargetModeType.AutoQuest, TargetModeType.Infer });
    private static readonly Lazy<object[]> WorldValues = new(() => new object[] { TargetModeType.LockOn, TargetModeType.MapPin, TargetModeType.Infer });

    public object[] GetValues(GameProcessType game)
    {
        return game switch
        {
            GameProcessType.MonsterHunterRise => RiseValues.Value,
            GameProcessType.MonsterHunterWorld => WorldValues.Value,
            GameProcessType.None => Array.Empty<object>(),
            GameProcessType.All => Array.Empty<object>(),
            _ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
        };
    }
}