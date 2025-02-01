using HunterPie.Core.Client;
using HunterPie.Core.Observability.Logging.Internal;
using HunterPie.Core.Observability.Logging.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace HunterPie.Core.Observability.Logging;

public static class LoggerFactory
{
    private static readonly List<LogMessage> Messages = new();
    private static readonly List<ILogWriter> Writers = new();
    private static readonly object Lock = new();

    public static void Add(ILogWriter writer)
    {
        lock (Lock)
        {
            Writers.Add(writer);

            foreach (LogMessage message in Messages)
            {
                if (ClientConfig.Config.Development.ClientLogLevel > message.Level)
                    continue;

                Action<string> dispatcher = LoggingUtils.ResolveLoggingFunction(message.Type, writer);
                dispatcher(message.Message);
            }
        }
    }

    public static ILogger Create([CallerFilePath] string tag = "")
    {
        tag = Path.GetFileNameWithoutExtension(tag);
        tag = tag.EndsWith(".xaml")
            ? tag[0..^5]
            : tag;

        return new TaggeableLogger(
            @lock: Lock,
            messages: Messages,
            loggers: Writers,
            tag: tag
        );
    }
}