using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Logging.Services;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class HunterPieLoggerInitializer : IInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly HunterPieLogWriter _logWriter;

    public HunterPieLoggerInitializer(HunterPieLogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    public Task Init()
    {
        LoggerFactory.Add(_logWriter);

        _logger.Info("Initialized HunterPie logger");

        return Task.CompletedTask;
    }
}