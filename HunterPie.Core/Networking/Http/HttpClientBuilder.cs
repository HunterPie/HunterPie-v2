using HunterPie.Core.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace HunterPie.Core.Networking.Http;

#nullable enable
public class HttpClientBuilder
{

    private HttpMethod _method = HttpMethod.Get;
    private string _path = string.Empty;
    private HttpContent? _content = null;
    private TimeSpan _timeout = TimeSpan.MaxValue;
    private readonly List<string> _urls = new();
    private readonly Dictionary<string, string?> _headers = new();
    private int _retry = 1;
    private string _query = string.Empty;

    public HttpClientBuilder() { }

    public HttpClientBuilder(string[] urls)
    {
        _urls.AddRange(urls);
    }

    public HttpClientBuilder(string url)
    {
        _urls.Add(url);
    }

    public HttpClientBuilder Get(string path)
    {
        _method = HttpMethod.Get;
        _path = path;

        return this;
    }

    public HttpClientBuilder Post(string path)
    {
        _method = HttpMethod.Post;
        _path = path;

        return this;
    }

    public HttpClientBuilder Delete(string path)
    {
        _method = HttpMethod.Delete;
        _path = path;

        return this;
    }

    public HttpClientBuilder Patch(string path)
    {
        _method = HttpMethod.Patch;
        _path = path;

        return this;
    }

    public HttpClientBuilder WithQuery(Dictionary<string, object> parameters)
    {
        IEnumerable<string> encodedStrings = parameters.Select(it => $"{it.Key}={HttpUtility.UrlEncode(it.Value.ToString())}");

        _query = "?" + string.Join("&", encodedStrings);

        return this;
    }

    public HttpClientBuilder WithHeader(string key, string? value)
    {
        if (value is { } header)
            _headers.Add(key, header.Replace("\n", string.Empty));

        return this;
    }

    public HttpClientBuilder WithJson<T>(T json)
    {
        string serialized = JsonProvider.Serializer(json);
        _content = new StringContent(serialized, Encoding.UTF8, "application/json");

        return this;
    }

    public HttpClientBuilder WithFile(string name, string path)
    {
        Stream fileStream = File.OpenRead(path);
        var content = new StreamContent(fileStream);
        var form = new MultipartFormDataContent { Headers = { ContentType = { MediaType = "multipart/form-data" } } };
        form.Add(content, name, name);
        _content = form;

        return this;
    }

    public HttpClientBuilder WithTimeout(TimeSpan timeout)
    {
        _timeout = timeout;

        return this;
    }

    public HttpClientBuilder WithRetry(int retryCount)
    {
        _retry = retryCount;

        return this;
    }

    public HttpClient Build() => new()
    {
        Content = _content,
        Headers = _headers,
        Urls = _urls,
        Method = _method,
        Path = _path,
        Retry = _retry,
        TimeOut = _timeout,
        Query = _query
    };
}