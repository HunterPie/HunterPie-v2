using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Patch.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Patch;
internal class PoogiePatchConnector
{
    private readonly IPoogieClientAsync _client;

    private const string PATCH_ENDPOINT = "/v1/patch/notes";

    public PoogiePatchConnector(IPoogieClientAsync client)
    {
        _client = client;
    }

    public async Task<PoogieResult<PatchResponse[]>> FindAll() =>
        await _client.GetAsync<PatchResponse[]>(PATCH_ENDPOINT);
}