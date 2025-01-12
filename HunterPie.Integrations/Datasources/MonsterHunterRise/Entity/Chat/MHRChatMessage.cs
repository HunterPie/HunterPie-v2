using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Chat;

public class MHRChatMessage : IChatMessage
{
    public string Message { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public AuthorType Type { get; init; }

    public int PlayerSlot { get; init; }
}