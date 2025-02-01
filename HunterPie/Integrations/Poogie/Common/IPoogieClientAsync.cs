using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Common;

internal interface IPoogieClientAsync
{
    public Task<PoogieResult<T>> GetAsync<T>(string path, Dictionary<string, object>? query = null);
    public Task<HttpClientResponse> DownloadAsync(string path);
    public Task<PoogieResult<TOut>> PostAsync<TIn, TOut>(string path, TIn payload);
    public Task<PoogieResult<T>> SendFileAsync<T>(string path, string filename);
    public Task<PoogieResult<TOut>> DeleteAsync<TIn, TOut>(string path, TIn payload);
    public Task<PoogieResult<TOut>> PatchAsync<TIn, TOut>(string path, TIn payload);
}