using HunterPie.Core.Client;
using HunterPie.Core.Networking.Http;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.UI.Architecture;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie.Features.Backup.ViewModels;

internal class BackupElementViewModel : ViewModel
{
    private readonly PoogieBackupConnector _backupConnector;

    private string _backupId = string.Empty;
    public string BackupId { get => _backupId; set => SetValue(ref _backupId, value); }

    private string _gameName = string.Empty;
    public string GameName { get => _gameName; set => SetValue(ref _gameName, value); }

    private string _gameIcon = string.Empty;
    public string GameIcon { get => _gameIcon; set => SetValue(ref _gameIcon, value); }

    private DateTime _uploadedAt;
    public DateTime UploadedAt { get => _uploadedAt; set => SetValue(ref _uploadedAt, value); }

    private long _byteSize;
    public long ByteSize { get => _byteSize; set => SetValue(ref _byteSize, value); }

    private bool _isDownloading;
    public bool IsDownloading { get => _isDownloading; set => SetValue(ref _isDownloading, value); }

    private double _bytesDownloaded;
    public double BytesDownloaded { get => _bytesDownloaded; set => SetValue(ref _bytesDownloaded, value); }

    private double _bytesToDownload;
    public double BytesToDownload { get => _bytesToDownload; set => SetValue(ref _bytesToDownload, value); }

    private bool _isDownloaded;
    public bool IsDownloaded { get => _isDownloaded; set => SetValue(ref _isDownloaded, value); }

    public BackupElementViewModel(PoogieBackupConnector backupConnector)
    {
        _backupConnector = backupConnector;
    }

    public async Task Download()
    {
        IsDownloading = true;

        using HttpClientResponse response = await _backupConnector.DownloadAsync(BackupId);


        if (!response.Success)
        {
            IsDownloading = false;
            return;
        }

        response.OnDownloadProgressChanged += (_, args) =>
        {
            BytesDownloaded = args.BytesDownloaded;
            BytesToDownload = args.TotalBytes;
        };

        await response.DownloadAsync(
            outPath: ClientInfo.GetPathFor($"Backups/{BackupId}.zip")
        );

        IsDownloading = false;
        IsDownloaded = true;

        var options = new NotificationOptions(
            Type: NotificationType.Success,
            Title: "Download",
            Description: "Successfully downloaded backup",
            DisplayTime: TimeSpan.FromSeconds(5)
        );
        await NotificationService.Show(options);
    }

    public void OpenBackupFolder()
    {
        Process.Start("explorer", ClientInfo.GetPathFor("Backups"));
    }

    public async Task Delete()
    {
        IsDownloading = true;
        await _backupConnector.DeleteAsync(BackupId);
        IsDownloading = false;
    }
}