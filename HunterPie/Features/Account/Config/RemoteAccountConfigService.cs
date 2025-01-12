using HunterPie.Core.Base64;
using HunterPie.Core.Client;
using HunterPie.Core.Json;
using HunterPie.Core.Logger;
using HunterPie.Features.Account.UseCase;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Settings;
using HunterPie.Integrations.Poogie.Settings.Models;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Config;

internal class RemoteAccountConfigService : IRemoteAccountConfigUseCase
{
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

    public async Task UploadAsync()
    {
        if (!await _accountUseCase.IsValidSessionAsync())
            return;

        IAbstractHunterPieConfig config = ClientConfig.Config;
        string serializedConfig = JsonProvider.Serializer(config);
        string encodedConfig = Base64Service.Encode(serializedConfig);

        PoogieResult<ClientSettingsResponse> result = await _settingsConnector.UploadClientSettings(
            request: new ClientSettingsRequest(encodedConfig)
        );

        if (result.Response is not { } response)
            return;

        Log.Debug("Uploaded config with length {0}", response.Configuration.Length);
    }

    public async Task DownloadAsync()
    {
        if (!await _accountUseCase.IsValidSessionAsync())
            return;

        PoogieResult<ClientSettingsResponse> result = await _settingsConnector.GetClientSettings();

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