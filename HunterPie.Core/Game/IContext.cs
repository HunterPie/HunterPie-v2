using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game.Entity.Game;

namespace HunterPie.Core.Game;
public interface IContext
{
    public IGame Game { get; }
    public IGameProcess Process { get; }
}