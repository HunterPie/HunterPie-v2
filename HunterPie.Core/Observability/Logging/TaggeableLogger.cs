using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Observability.Logging.Entity;
using HunterPie.Core.Observability.Logging.Internal;
using HunterPie.Core.Observability.Logging.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Observability.Logging;

internal class TaggeableLogger : ILogger
{
    private readonly string _tag;
    private readonly object _lock;
    private readonly List<LogMessage> _messages;
    private readonly List<ILogWriter> _loggers;

    public TaggeableLogger(
        object @lock,
        List<LogMessage> messages,
        List<ILogWriter> loggers,
        string tag)
    {
        _lock = @lock;
        _messages = messages;
        _tag = tag;
        _loggers = loggers;
    }

    public void Debug(string message) =>
        Write(LogLevel.Debug, LogType.Debug, message);

    public void Info(string message) =>
        Write(LogLevel.Info, LogType.Info, message);

    public void Warning(string message) =>
        Write(LogLevel.Warn, LogType.Warning, message);

    public void Native(string message) =>
        Write(LogLevel.Info, LogType.Native, message);

    public void Error(string message) =>
        Write(LogLevel.Error, LogType.Error, message);

    public void Benchmark(string message) =>
        Write(LogLevel.Info, LogType.Benchmark, message);

    private void Write(LogLevel level, LogType type, string message)
    {
        message = $"[{_tag}] {message}";

        lock (_lock)
        {
            _messages.Add(new LogMessage
            {
                Level = level,
                Type = type,
                Message = message
            });

            if (!ClientConfig.IsInitialized)
                return;

            if (ClientConfig.Config.Development.ClientLogLevel > level)
                return;

            IEnumerable<Action<string>> loggers = _loggers.Select(it => LoggingUtils.ResolveLoggingFunction(type, it));

            foreach (Action<string> log in loggers)
                log(message);
        }
    }
}