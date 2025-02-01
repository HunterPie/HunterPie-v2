namespace HunterPie.UI.Logging.Entity;

public class LogString
{
    public required string Message { get; init; }
    public required string Timestamp { get; init; }
    public required LogLevel Level { get; init; }
}