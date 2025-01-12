using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InternalHttpClient = System.Net.Http.HttpClient;

namespace HunterPie.Core.Networking.Http;

public class HttpClient : IDisposable
{

    private InternalHttpClient? _httpClient;
    private HttpRequestMessage? _request;

    public IReadOnlyList<string> Urls { get; init; } = new List<string>();
    public string? Path { get; init; }
    public HttpMethod? Method { get; init; }
    public HttpContent? Content { get; init; }
    public TimeSpan TimeOut { get; init; } = TimeSpan.MaxValue;
    public int Retry { get; init; } = 1;
    public IReadOnlyDictionary<string, string?> Headers { get; init; } = new Dictionary<string, string?>();
    public string Query { get; init; } = string.Empty;

    public HttpClientResponse Request()
    {
        foreach (string host in Urls)
        {
            Log.Debug($"Making request to {Path}");
            for (int retry = 0; retry < Math.Max(1, Retry); retry++)
            {
                _httpClient = new() { Timeout = TimeOut };
                _request = new(Method!, $"{host}{Path}{Query}");

                if (Content is not null)
                    _request.Content = Content;

                foreach ((string key, string value) in
                         Headers.Where(k => k.Value is not null)
                             .Cast<KeyValuePair<string, string>>()
                        )
                    _request.Headers.Add(key, value);

                HttpResponseMessage response;
                try
                {
                    response = _httpClient.Send(_request, HttpCompletionOption.ResponseHeadersRead);
                }
                catch (Exception err)
                {
                    Log.Debug(err.ToString());
                    _httpClient.Dispose();
                    _request.Dispose();

                    continue;
                }

                return new HttpClientResponse(response);
            }
        }


        return new HttpClientResponse(null);
    }

    public async Task<HttpClientResponse> RequestAsync()
    {
        foreach (string host in Urls)
        {
            Log.Debug($"Making request to {Path}");
            for (int retry = 0; retry < Math.Max(1, Retry); retry++)
            {
                _httpClient = new() { Timeout = TimeOut };
                _request = new(Method!, $"{host}{Path}{Query}");

                if (Content is not null)
                    _request.Content = Content;

                foreach ((string key, string value) in
                         Headers.Where(k => k.Value is not null)
                             .Cast<KeyValuePair<string, string>>()
                        )
                    _request.Headers.Add(key, value);

                HttpResponseMessage response;
                try
                {
                    response = await _httpClient.SendAsync(_request, HttpCompletionOption.ResponseHeadersRead);
                }
                catch (Exception err)
                {
                    Log.Debug(err.ToString());
                    _httpClient.Dispose();
                    _request.Dispose();

                    continue;
                }

                return new HttpClientResponse(response);
            }
        }


        return new HttpClientResponse(null);
    }

    public void Dispose()
    {
        Content?.Dispose();
        _httpClient?.Dispose();
        _request?.Dispose();
    }
}