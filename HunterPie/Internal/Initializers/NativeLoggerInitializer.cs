using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features.Repository;
using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.Platforms.Common.Logging;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NativeLoggerInitializer : IInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly INativeLogWriter _logWriter;
    private readonly IFeatureFlagRepository _featureFlagRepository;

    public NativeLoggerInitializer(
        INativeLogWriter logWriter,
        IFeatureFlagRepository featureFlagRepository)
    {
        _logWriter = logWriter;
        _featureFlagRepository = featureFlagRepository;
    }

    public Task Init()
    {
        if (!_featureFlagRepository.IsEnabled(FeatureFlags.FEATURE_NATIVE_LOGGER))
            return Task.CompletedTask;

        _logWriter.CreateTerminal();
        LoggerFactory.Add(_logWriter);

        _logger.Info("Hello world! HunterPie native logger has been initialized!");

        return Task.CompletedTask;
    }
}