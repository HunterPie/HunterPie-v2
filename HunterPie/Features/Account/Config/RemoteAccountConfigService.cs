using HunterPie.Core.Base64;
using HunterPie.Core.Client;
using HunterPie.Core.Json;
using HunterPie.Core.Observability.Logging;
using HunterPie.Features.Account.UseCase;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Settings;
using HunterPie.Integrations.Poogie.Settings.Models;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Config;

internal class RemoteAccountConfigService : IRemoteAccountConfigUseCase
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly IAccountUseCase _accountUseCase;
    private readonly PoogieClientSettingsConnector _settingsConnector;

    public RemoteAccountConfigService(
        IAccountUseCase accountUseCase,
        PoogieClientSettingsConnector settingsConnector
    )
    {
        _accountUseCase = accountUseCase;
        _settingsConnector = settingsConnector;
    }

    public async Task Upload()
    {
        if (!await _accountUseCase.IsValidSessionAsync())
            return;

        IAbstractHunterPieConfig config = ClientConfig.Config;
        string serializedConfig = JsonProvider.Serializer(config);
        string encodedConfig = Base64Service.Encode(serializedConfig);

        PoogieResult<ClientSettingsResponse> result = await _settingsConnector.UploadClientSettingsAsync(
            request: new ClientSettingsRequest(encodedConfig)
        );

        if (result.Response is not { } response)
            return;

        _logger.Debug($"Uploaded config with length {response.Configuration.Length}");
    }

    public async Task Download()
    {
        if (!await _accountUseCase.IsValidSessionAsync())
            return;

        PoogieResult<ClientSettingsResponse> result = await _settingsConnector.GetClientSettingsAsync();

        if (result.Response is not { } response)
            return;

        if (result.Error is { Code: PoogieErrorCode.NOT_FOUND })
            return;

        string decodedConfig = Base64Service.Decode(response.Configuration);
        object config = JsonProvider.Deserializer(decodedConfig, ClientConfig.Config.GetType());

        ConfigHelper.WriteObject(
            path: ConfigHelper.GetFullPath(ClientConfig.CONFIG_NAME),
            obj: config
        );
    }
}