using HunterPie.Core.Observability.Logging;

namespace HunterPie.Platforms.Common.Logging;

internal interface INativeLogWriter : ILogWriter
{
    public void CreateTerminal();
}