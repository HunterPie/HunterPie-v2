using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Json;
using HunterPie.Core.Networking.Http.Events;
using HunterPie.Core.Observability.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Core.Networking.Http;

#nullable enable
public class HttpClientResponse : IEventDispatcher, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly HttpResponseMessage? _response;

    public HttpStatusCode? StatusCode { get; }
    public HttpContent? Content { get; }
    public bool Success { get; }
    public string? Url { get; }

    public event EventHandler<DownloadEventArgs>? OnDownloadProgressChanged;

    public HttpClientResponse(HttpResponseMessage? message)
    {
        if (message is null)
        {
            Success = false;
            return;
        }

        _response = message;
        StatusCode = message.StatusCode;
        Content = message.Content;
        Success = true;
        Url = message.RequestMessage!.RequestUri!.AbsoluteUri;
    }

    public async Task<T?> AsJsonAsync<T>()
    {
        if (Content is null)
            return default;

        string content = await Content.ReadAsStringAsync();

        try
        {
            return JsonProvider.Deserializer<T>(content);
        }
        catch (Exception err)
        {
            _logger.Error(err.ToString());
            return default;
        }
    }

    public async Task<byte[]?> AsRawAsync() => Content is { } content ? await content.ReadAsByteArrayAsync() : null;

    public async Task<string?> AsTextAsync() => Content is { } content ? await content.ReadAsStringAsync() : null;

    public async Task DownloadAsync(string outPath)
    {
        long totalBytes = (long)Content!.Headers!.ContentLength!;

        await using Stream stream = await Content!.ReadAsStreamAsync();
        long totalBytesRead = 0;
        byte[] buffer = new byte[8192];

        try
        {
            string directoryPath = Path.GetDirectoryName(outPath)!;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            await using var output = new FileStream(
                outPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                buffer.Length,
                useAsync: true
            );

            do
            {
                int bytesRead = await stream.ReadAsync(buffer);

                if (bytesRead is 0)
                {
                    this.Dispatch(OnDownloadProgressChanged, new(totalBytesRead, totalBytes));
                    break;
                }

                await output.WriteAsync(buffer.AsMemory(0, bytesRead));

                totalBytesRead += bytesRead;

                this.Dispatch(OnDownloadProgressChanged, new(totalBytesRead, totalBytes));
            } while (true);
        }
        catch (Exception err)
        {
            _logger.Error(err.ToString());
        }
    }

    public void Dispose()
    {
        Content?.Dispose();
        _response?.Dispose();
    }
}