using HunterPie.Core.Client;
using HunterPie.Core.Networking.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Remote;

public class CDN
{
    private const string CDN_BASE_URL = "https://cdn.hunterpie.com";

    private static readonly HashSet<string> NotFoundCache = new();

    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    public static async Task<string> GetMonsterIconUrl(string imageName)
    {
        if (NotFoundCache.Contains(imageName))
            return null;

        string localImage = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{imageName}.png");

        await semaphore.WaitAsync();

        try
        {
            if (File.Exists(localImage))
                return localImage;

            using HttpClient client = new HttpClientBuilder(CDN_BASE_URL)
                .Get($"/Assets/Monsters/Icons/{imageName}.png")
                .WithTimeout(TimeSpan.FromSeconds(5))
                .Build();

            using HttpClientResponse response = await client.RequestAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _ = NotFoundCache.Add(imageName);
                return null;
            }

            await response.DownloadAsync(localImage);
        }
        finally
        {
            semaphore.Release();
        }

        return localImage;
    }

    public static async Task GetFile(string path, string outPath)
    {
        using HttpClient request = new HttpClientBuilder(CDN_BASE_URL)
            .Get(path)
            .WithTimeout(TimeSpan.FromSeconds(6))
            .WithRetry(3)
            .Build();

        using HttpClientResponse response = await request.RequestAsync();

        if (response.StatusCode != HttpStatusCode.OK)
            return;

        await response.DownloadAsync(outPath);
    }

    public static async Task<string> GetAsset(string uri)
    {
        string fileName = Path.GetFileName(uri);
        string localImage = ClientInfo.GetPathFor($"Assets/Cache/{fileName}");

        if (File.Exists(localImage))
            return localImage;

        string filePath = uri.Replace(CDN_BASE_URL, string.Empty);

        using HttpClient request = new HttpClientBuilder(CDN_BASE_URL)
            .Get(filePath)
            .WithTimeout(TimeSpan.FromSeconds(5))
            .Build();

        using HttpClientResponse response = await request.RequestAsync();

        if (response.StatusCode != HttpStatusCode.OK)
            return null;

        await response.DownloadAsync(localImage);

        return localImage;
    }
}