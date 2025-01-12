using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;

namespace HunterPie.Integrations.Services;

public interface IGameContextService
{
    public Context Get(IGameProcess game);
}