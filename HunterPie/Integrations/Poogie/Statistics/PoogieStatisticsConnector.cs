using HunterPie.Core.Domain.Cache;
using HunterPie.Core.Domain.Cache.Model;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Statistics;

internal class PoogieStatisticsConnector
{
    private const string SUMMARIES_CACHE_KEY = "summaries";
    private const string UPLOAD_CACHE_KEY = "quest::{0}";

    private readonly IAsyncCache _cache;
    private readonly IPoogieClientAsync _client;

    private const string STATISTICS_ENDPOINT = "/v1/hunt";
    private const string STATISTICS_ENDPOINT_V2 = "/v2/hunt";

    public PoogieStatisticsConnector(
        IAsyncCache cache,
        IPoogieClientAsync client)
    {
        _cache = cache;
        _client = client;
    }

    public async Task<PoogieResult<PoogieQuestStatisticsModel>> UploadAsync(PoogieQuestStatisticsModel model) =>
        await _client.PostAsync<PoogieQuestStatisticsModel, PoogieQuestStatisticsModel>($"{STATISTICS_ENDPOINT}/upload", model);

    public async Task<PoogieResult<Paginated<PoogieQuestSummaryModel>>> GetUserQuestSummariesV2(int page, int limit)
    {
        PoogieResult<Paginated<PoogieQuestSummaryModel>> result =
            await _client.GetAsync<Paginated<PoogieQuestSummaryModel>>(
                path: STATISTICS_ENDPOINT_V2,
                query: new Dictionary<string, object>
                {
                    { nameof(page), page },
                    { nameof(limit), limit}
                });

        return result;
    }

    public async Task<PoogieResult<List<PoogieQuestSummaryModel>>> GetUserQuestSummariesAsync()
    {
        PoogieResult<List<PoogieQuestSummaryModel>>? cachedSummaries =
            await _cache.GetAsync<PoogieResult<List<PoogieQuestSummaryModel>>>(SUMMARIES_CACHE_KEY);

        if (cachedSummaries != null)
            return cachedSummaries;

        PoogieResult<List<PoogieQuestSummaryModel>> result = await _client.GetAsync<List<PoogieQuestSummaryModel>>($"{STATISTICS_ENDPOINT}");

        if (result.Response is not null)
            await _cache.SetAsync(
                key: SUMMARIES_CACHE_KEY,
                value: result,
                options: new CacheOptions(Ttl: TimeSpan.FromMinutes(1))
            );

        return result;
    }

    public async Task<PoogieResult<PoogieQuestStatisticsModel>> GetAsync(string uploadId)
    {
        PoogieResult<PoogieQuestStatisticsModel>? cachedStatistics =
            await _cache.GetAsync<PoogieResult<PoogieQuestStatisticsModel>>(string.Format(UPLOAD_CACHE_KEY, uploadId));

        if (cachedStatistics != null)
            return cachedStatistics;

        PoogieResult<PoogieQuestStatisticsModel> result = await _client.GetAsync<PoogieQuestStatisticsModel>($"{STATISTICS_ENDPOINT}/{uploadId}");

        if (result.Response is not null)
            await _cache.SetAsync(
                key: string.Format(UPLOAD_CACHE_KEY, uploadId),
                value: result
            );

        return result;
    }
}