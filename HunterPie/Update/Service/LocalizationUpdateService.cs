using HunterPie.Core.Client;
using HunterPie.Core.Crypto;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Remote;
using HunterPie.Update.Gateway;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Update.Service;

internal class LocalizationUpdateService
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly UpdateGateway _gateway;

    public LocalizationUpdateService(UpdateGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task InvokeAsync()
    {
        Dictionary<string, string> latestChecksum = await _gateway.GetLocalizationsChecksumAsync();

        foreach ((string name, string checksum) in latestChecksum)
        {

            string fileName = name.Replace("localization/", string.Empty);
            string localFilePath = ClientInfo.GetPathFor($"Languages/{fileName}");

            string localChecksum = File.Exists(localFilePath)
                ? await HashService.ChecksumAsync(localFilePath)
                : string.Empty;

            if (checksum == localChecksum)
                continue;

            _logger.Debug($"Downloading {name}... Remote checksum: {checksum} | Local checksum: {localChecksum}");
            await CDN.GetFile($"/{name}", localFilePath);
        }
    }
}