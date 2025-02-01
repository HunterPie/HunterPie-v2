using DiscordRPC;

namespace HunterPie.Integrations.Discord.Strategies;

internal interface IDiscordRichPresenceStrategy
{
    public string AppId { get; }

    public void Update(RichPresence presence);
}