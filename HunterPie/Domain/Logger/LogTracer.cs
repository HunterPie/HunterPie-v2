using HunterPie.Core.Observability.Logging;
using System.Diagnostics;

namespace HunterPie.Domain.Logger;

internal class LogTracer : TraceListener
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public override void Write(string message)
    {

    }

    public override void WriteLine(string message) => _logger.Error(message);
}