namespace HunterPie.Core.Observability.Logging;

public interface ILogger
{
    public void Debug(string message);
    public void Info(string message);
    public void Warning(string message);
    public void Native(string message);
    public void Error(string message);
    public void Benchmark(string message);
}