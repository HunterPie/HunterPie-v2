using System;

namespace HunterPie.Core.Extensions;
public static class DateTimeExtensions
{
    public static DateTime ToLocalTime(this DateTime time)
    {
        return time - (DateTime.UtcNow - DateTime.Now);
    }

    public static bool HasElapsed(this DateTime time, DateTime other, TimeSpan span) =>
        (time - other).TotalMilliseconds >= span.TotalMilliseconds;

    /// <summary>
    /// Returns whether this time is between two times (inclusive)
    /// </summary>
    /// <param name="time">Time</param>
    /// <param name="start">Start time</param>
    /// <param name="end">End time</param>
    /// <returns>True if time is between start and end, false otherwise</returns>
    public static bool IsBetween(this DateTime time, DateTime start, DateTime end)
    {
        return time >= start && time <= end;
    }

    public static DateTime Max(this DateTime time, DateTime other)
    {
        return time > other ? time : other;
    }
}