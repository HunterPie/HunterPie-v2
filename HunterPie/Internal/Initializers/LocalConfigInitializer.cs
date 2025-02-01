using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Settings.Types;
using HunterPie.Domain.Interfaces;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class LocalConfigInitializer : IInitializer
{
    private readonly ILocalRegistryAsync _localRegistryAsync;

    private const string KEY_SECRET = "secret";
    private const string CLIENT_ID = "client_id";

    public LocalConfigInitializer(ILocalRegistryAsync localRegistryAsync)
    {
        _localRegistryAsync = localRegistryAsync;
    }

    public async Task Init()
    {
        using var rng = RandomNumberGenerator.Create();

        await GenerateSecretKeyAsync(rng);
        await GenerateClientIdAsync(rng);
    }

    /// <summary>
    /// Generates a new Secret Key, this will be used SOLELY to keep string values safe in the config.json when using the <seealso cref="Secret"/>
    /// class, so users can share their config.json without the risk of losing whatever was saved in the Secret text box.
    /// </summary>
    private async Task GenerateSecretKeyAsync(RandomNumberGenerator rng)
    {
        if (await _localRegistryAsync.ExistsAsync(KEY_SECRET))
            return;

        byte[] token = new byte[16];
        rng.GetBytes(token);

        await _localRegistryAsync.SetAsync(KEY_SECRET, token);
    }

    /// <summary>
    /// Generates a new Client Id for this HunterPie client, this will be used in Http requests to the API
    /// </summary>
    private async Task GenerateClientIdAsync(RandomNumberGenerator rng)
    {
        if (await _localRegistryAsync.ExistsAsync(CLIENT_ID))
            return;

        byte[] bytes = new byte[16];
        rng.GetBytes(bytes);

        string token = BitConverter.ToString(bytes)
            .Replace("-", string.Empty);

        await _localRegistryAsync.SetAsync(CLIENT_ID, token);
    }
}