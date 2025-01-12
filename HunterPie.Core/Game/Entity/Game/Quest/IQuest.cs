using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Game.Quest;

public interface IQuest
{
    /// <summary>
    /// Id of the current quest
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Name of the quest
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Type of the quest
    /// </summary>
    public QuestType Type { get; }

    /// <summary>
    /// Status of the current quest
    /// </summary>
    public QuestStatus Status { get; }

    /// <summary>
    /// Number of currents deaths
    /// </summary>
    public int Deaths { get; }

    /// <summary>
    /// Number of maximum deaths
    /// </summary>
    public int MaxDeaths { get; }

    /// <summary>
    /// General level of the quest
    /// </summary>
    public QuestLevel Level { get; }

    /// <summary>
    /// Exact number of stars of the quest
    /// </summary>
    public int Stars { get; }

    /// <summary>
    /// Quest time left
    /// </summary>
    public TimeSpan TimeLeft { get; }

    /// <summary>
    /// Event dispatched whenever the quest status change
    /// </summary>
    public event EventHandler<SimpleValueChangeEventArgs<QuestStatus>> OnQuestStatusChange;

    /// <summary>
    /// Event dispatched whenever someone dies
    /// </summary>
    public event EventHandler<CounterChangeEventArgs> OnDeathCounterChange;

    /// <summary>
    /// Event dispatched whenever the time left is updated
    /// </summary>
    public event EventHandler<SimpleValueChangeEventArgs<TimeSpan>> OnTimeLeftChange;
}