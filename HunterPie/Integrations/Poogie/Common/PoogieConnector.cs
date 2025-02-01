using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Common;

internal class PoogieConnector : IPoogieClientAsync
{
    private readonly PoogieHttpProvider _poogieHttpProvider;

    public PoogieConnector(PoogieHttpProvider poogieHttpProvider)
    {
        _poogieHttpProvider = poogieHttpProvider;
    }

    public async Task<PoogieResult<T>> GetAsync<T>(string path, Dictionary<string, object>? query = null)
    {
        HttpClientBuilder clientBuilder = _poogieHttpProvider.Default()
            .Get(path);

        if (query is not null)
            clientBuilder.WithQuery(query);

        using HttpClient client = clientBuilder.Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<T>.FromAsync(response);
    }

    public async Task<HttpClientResponse> DownloadAsync(string path)
    {
        using HttpClient client = _poogieHttpProvider.Default()
            .Get(path)
            .WithTimeout(TimeSpan.FromSeconds(60))
            .Build();

        return await client.RequestAsync();
    }

    public async Task<PoogieResult<TOut>> PostAsync<TIn, TOut>(string path, TIn payload)
    {
        using HttpClient client = _poogieHttpProvider.Default()
            .Post(path)
            .WithJson(payload)
            .Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<TOut>.FromAsync(response);
    }

    public async Task<PoogieResult<T>> SendFileAsync<T>(string path, string filename)
    {
        using HttpClient client = _poogieHttpProvider.Default()
            .Post(path)
            .WithTimeout(TimeSpan.FromSeconds(60))
            .WithFile("file", filename)
            .Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<T>.FromAsync(response);
    }

    public async Task<PoogieResult<TOut>> DeleteAsync<TIn, TOut>(string path, TIn payload)
    {
        using HttpClient client = _poogieHttpProvider.Default()
            .Delete(path)
            .WithJson(payload)
            .Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<TOut>.FromAsync(response);
    }

    public async Task<PoogieResult<TOut>> PatchAsync<TIn, TOut>(string path, TIn payload)
    {
        using HttpClient client = _poogieHttpProvider.Default()
            .Patch(path)
            .WithJson(payload)
            .Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<TOut>.FromAsync(response);
    }
}