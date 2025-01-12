using HunterPie.Core.Client.Configuration.Enums;
using System.Threading.Tasks;

namespace HunterPie.Features.Backup.Services;

internal interface IBackupService
{
    /// <summary>
    /// Executes a backup and returns the path to the backup file
    /// </summary>
    /// <returns>Path to the backup file</returns>
    public Task ExecuteAsync(GameType gameType);
}