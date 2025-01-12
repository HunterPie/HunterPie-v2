using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Version.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Version;
internal class PoogieVersionConnector
{
    private readonly IPoogieClientAsync _client;

    private const string VERSION_ENDPOINT = "/v1/version";

    public PoogieVersionConnector(IPoogieClientAsync client)
    {
        _client = client;
    }

    public async Task<PoogieResult<VersionResponse>> Latest() =>
        await _client.GetAsync<VersionResponse>(VERSION_ENDPOINT);

    public async Task<HttpClientResponse> Download(string version) =>
        await _client.DownloadAsync($"{VERSION_ENDPOINT}/{version}");
}