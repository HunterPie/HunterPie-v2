using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Supporter.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Supporter;
internal class PoogieSupporterConnector
{
    private readonly IPoogieClientAsync _client;

    private const string SUPPORTER_ENDPOINT = "/v1/supporter";

    public PoogieSupporterConnector(IPoogieClientAsync client)
    {
        _client = client;
    }

    public async Task<PoogieResult<SupporterValidationResponse>> IsValidSupporterAsync() =>
        await _client.GetAsync<SupporterValidationResponse>($"{SUPPORTER_ENDPOINT}/verify");
}