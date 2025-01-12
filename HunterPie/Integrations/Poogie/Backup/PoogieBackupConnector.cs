using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie.Backup.Models;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Backup;
internal class PoogieBackupConnector
{
    private const string BACKUP_ENDPOINT = "/v1/backup";
    private readonly IPoogieClientAsync _client;

    public PoogieBackupConnector(IPoogieClientAsync client)
    {
        _client = client;
    }

    public async Task<PoogieResult<CanUploadBackupResponse>> CanUploadBackupAsync() =>
        await _client.GetAsync<CanUploadBackupResponse>($"{BACKUP_ENDPOINT}/upload");

    public async Task<PoogieResult<UserBackupDetailsResponse>> FindAllAsync() =>
        await _client.GetAsync<UserBackupDetailsResponse>(BACKUP_ENDPOINT);

    public async Task<PoogieResult<BackupResponse>> UploadAsync(GameType gameType, string filename) =>
        await _client.SendFileAsync<BackupResponse>($"{BACKUP_ENDPOINT}/upload/{gameType}", filename);

    public async Task<HttpClientResponse> DownloadAsync(string backupId) =>
        await _client.DownloadAsync($"{BACKUP_ENDPOINT}/{backupId}");

    public async Task<PoogieResult<BackupDeleteResponse>> DeleteAsync(string backupId) =>
        await _client.DeleteAsync<Nothing, BackupDeleteResponse>($"{BACKUP_ENDPOINT}/{backupId}", new Nothing());
}