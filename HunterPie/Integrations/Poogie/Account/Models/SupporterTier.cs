using HunterPie.Features.Account.Model;
using System;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal enum SupporterTier
{
    TIER_NONE,
    TIER1,
    TIER2,
    TIER3,
    TIER4,
    TIER_SPECIAl
}

internal static class SupporterTierExtensions
{
    public static AccountTier ToEntity(this SupporterTier tier) =>
        tier switch
        {
            SupporterTier.TIER_NONE => AccountTier.None,
            SupporterTier.TIER1 => AccountTier.LowRank,
            SupporterTier.TIER2 => AccountTier.HighRank,
            SupporterTier.TIER3 => AccountTier.Tempered,
            SupporterTier.TIER4 => AccountTier.ArchTempered,
            SupporterTier.TIER_SPECIAl => AccountTier.Special,
            _ => throw new ArgumentOutOfRangeException(nameof(tier), tier, null)
        };
}