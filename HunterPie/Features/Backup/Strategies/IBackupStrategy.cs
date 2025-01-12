using HunterPie.Integrations.Poogie.Common.Models;
using System.Threading.Tasks;

namespace HunterPie.Features.Backup.Strategies;

internal interface IBackupStrategy
{
    public GameType Type { get; }

    public Task<string> PackFilesAsync(string steamPath);
}