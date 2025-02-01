using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Discord.Service;
using HunterPie.Integrations.Discord.Strategies;
using HunterPie.Integrations.Services.Exceptions;

namespace HunterPie.Integrations.Discord.Factory;

internal class DiscordPresenceFactory
{
    private readonly ILocalizationRepository _localizationRepository;

    public DiscordPresenceFactory(ILocalizationRepository localizationRepository)
    {
        _localizationRepository = localizationRepository;
    }

    public DiscordPresenceService Create(IContext context)
    {
        DiscordRichPresence configuration = ClientConfigHelper.GetDiscordRichPresenceConfigFrom(context.Process.Type);

        IDiscordRichPresenceStrategy strategy = context switch
        {
            MHWContext => new MHWDiscordPresenceStrategy(
                localizationRepository: _localizationRepository,
                configuration: configuration,
                context: context
            ),
            MHRContext => new MHRDiscordPresenceStrategy(
                localizationRepository: _localizationRepository,
                configuration: configuration,
                context: context
            ),
            _ => throw new UnsupportedGameException($"Game {context.GetType().Name} is not supported")
        };

        return new DiscordPresenceService(
            context: context,
            configuration: configuration,
            strategy: strategy
        );
    }
}