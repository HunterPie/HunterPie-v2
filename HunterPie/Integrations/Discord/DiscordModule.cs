using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Discord.Factory;

namespace HunterPie.Integrations.Discord;

internal class DiscordModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<DiscordPresenceFactory>();
    }
}