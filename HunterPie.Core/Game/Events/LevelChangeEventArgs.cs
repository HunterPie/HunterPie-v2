using HunterPie.Core.Game.Entity.Player;
using System;

namespace HunterPie.Core.Game.Events;

public class LevelChangeEventArgs : EventArgs
{
    public int HighRank { get; }
    public int MasterRank { get; }

    public LevelChangeEventArgs(IPlayer player)
    {
        HighRank = player.HighRank;
        MasterRank = player.MasterRank;
    }
}