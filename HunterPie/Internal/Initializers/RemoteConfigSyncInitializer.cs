using HunterPie.Domain.Interfaces;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

/// <summary>
/// Initializer responsible for fetching and downloading the user's
/// remote config that's been uploaded in past sessions
/// </summary>
internal class RemoteConfigSyncInitializer : IInitializer
{
    private readonly IRemoteAccountConfigUseCase _remoteConfigService;
    private readonly RemoteConfigSyncService _configSyncService;

    public RemoteConfigSyncInitializer(
        IRemoteAccountConfigUseCase remoteConfigService,
        RemoteConfigSyncService configSyncService
        )
    {
        _remoteConfigService = remoteConfigService;
        _configSyncService = configSyncService;
    }

    public async Task Init()
    {
        await _remoteConfigService.DownloadAsync();

        _configSyncService.Start();
    }
}