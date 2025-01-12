using HunterPie.Core.Game.Entity.Game.Quest;
using System;

namespace HunterPie.Core.Game.Events;

public class QuestEndEventArgs : EventArgs
{
    public IQuest Quest { get; }

    public QuestStatus Status { get; }

    public TimeSpan TimeElapsed { get; }

    public QuestEndEventArgs(IQuest quest, QuestStatus status, float timeElapsed)
    {
        Quest = quest;
        Status = status;
        TimeElapsed = TimeSpan.FromSeconds(timeElapsed);
    }
}