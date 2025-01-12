using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Entity.Game.Chat;

public interface IChatMessage
{
    public string Message { get; }
    public string Author { get; }
    public AuthorType Type { get; }

    public int PlayerSlot { get; }
}