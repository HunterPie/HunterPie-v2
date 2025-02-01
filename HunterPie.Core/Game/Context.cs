using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game.Entity.Game;
using System;

namespace HunterPie.Core.Game;

public class Context : IContext, IDisposable
{
    public IGame Game { get; }
    public IGameProcess Process { get; }

    public Context(
        IGame game,
        IGameProcess process)
    {
        Game = game;
        Process = process;
    }

    public virtual void Dispose()
    {
        if (Game is IDisposable game)
            game.Dispose();

        GC.SuppressFinalize(this);
    }
}