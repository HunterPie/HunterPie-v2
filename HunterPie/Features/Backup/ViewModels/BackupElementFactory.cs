using HunterPie.Core.Client;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.Integrations.Poogie.Backup.Models;
using System.IO;

namespace HunterPie.Features.Backup.ViewModels;

internal class BackupElementFactory
{
    private readonly PoogieBackupConnector _backupConnector;

    public BackupElementFactory(PoogieBackupConnector backupConnector)
    {
        _backupConnector = backupConnector;
    }

    public BackupElementViewModel Create(BackupResponse response) =>
        new(
            backupConnector: _backupConnector
        )
        {
            BackupId = response.Id,
            GameName = response.GameName,
            GameIcon = response.GameIcon,
            ByteSize = response.Size,
            UploadedAt = response.UploadedAt.ToLocalTime(),
            IsDownloaded = File.Exists(
                ClientInfo.GetPathFor($"Backups/{response.Id}.zip")
            )
        };
}