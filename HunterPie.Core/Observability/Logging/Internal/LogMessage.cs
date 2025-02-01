using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Observability.Logging.Entity;

namespace HunterPie.Core.Observability.Logging.Internal;

public struct LogMessage
{
    public LogLevel Level;
    public LogType Type;
    public string Message;
}