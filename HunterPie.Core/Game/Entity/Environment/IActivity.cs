using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Entity.Environment;

public interface IActivity
{
    public ActivityType Type { get; }
}