using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using System;

namespace HunterPie.Integrations.Discord;

internal static class DiscordPresenceController
{

    public static RichPresence GetPresenceBy(IContext context)
    {
        return context switch
        {
            MHWContext ctx => new WorldRichPresence(ctx),
            MHRContext ctx => new RiseRichPresence(ctx),
            _ => throw new NotImplementedException("unreachable")
        };
    }
}