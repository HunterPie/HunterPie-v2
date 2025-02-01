using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Core.Observability.Logging;

namespace HunterPie.Internal.Logger;

internal class NativeLoggerFeature : Feature
{
    private readonly ILogger _logger = LoggerFactory.Create();

    protected override void OnEnable() => _logger.Debug("Enabled feature");

    protected override void OnDisable() => _logger.Debug("Disabled feature");
}