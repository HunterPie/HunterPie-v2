using HunterPie.Core.Observability.Logging.Entity;
using System;

namespace HunterPie.Core.Observability.Logging.Utils;

internal class LoggingUtils
{
    public static Action<string> ResolveLoggingFunction(LogType type, ILogger logger)
    {
        return type switch
        {
            LogType.Debug => logger.Debug,
            LogType.Info => logger.Info,
            LogType.Benchmark => logger.Benchmark,
            LogType.Native => logger.Native,
            LogType.Warning => logger.Warning,
            LogType.Error => logger.Error,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}