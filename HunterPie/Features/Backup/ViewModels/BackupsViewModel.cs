using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.Integrations.Poogie.Backup.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Backup.ViewModels;

internal class BackupsViewModel : ViewModel
{
    private readonly PoogieBackupConnector _backupConnector;
    private readonly AccountConfig _accountConfig;
    private readonly BackupElementFactory _elementFactory;

    public ObservableCollection<BackupElementViewModel> Backups { get; } = new();

    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    private int _maxCount;
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }

    private DateTime _lastSync;
    public DateTime LastSync { get => _lastSync; set => SetValue(ref _lastSync, value); }

    private bool _isFetching;
    public bool IsFetching { get => _isFetching; set => SetValue(ref _isFetching, value); }

    private bool _hasBackups;
    public bool HasBackups { get => _hasBackups; set => SetValue(ref _hasBackups, value); }

    public bool IsBackupEnabled => _accountConfig.IsBackupEnabled.Value;

    public BackupsViewModel(
        PoogieBackupConnector backupConnector,
        AccountConfig accountConfig,
        BackupElementFactory elementFactory)
    {
        _backupConnector = backupConnector;
        _accountConfig = accountConfig;
        _elementFactory = elementFactory;
    }

    public async Task FetchBackupsAsync()
    {
        IsFetching = true;
        PoogieResult<UserBackupDetailsResponse> backupDetails = await _backupConnector.FindAllAsync();
        IsFetching = false;

        if (backupDetails.Response is null)
            return;

        UserBackupDetailsResponse response = backupDetails.Response;

        Count = response.Count;
        MaxCount = response.MaxCount;
        HasBackups = response.Backups.Length > 0;
        LastSync = response.Backups.FirstOrDefault()?.UploadedAt.ToLocalTime() ?? DateTime.MinValue;

        await UIThread.InvokeAsync(() =>
        {
            response.Backups
                .Select(_elementFactory.Create)
                .ForEach(Backups.Add);
        });
    }

    private async void OnBackupDeleted(object? _, BackupDeleteResponse e)
    {
        await UIThread.InvokeAsync(() =>
        {
            BackupElementViewModel backup = Backups.First(vm => vm.BackupId == e.Id);

            Backups.Remove(backup);
        });

        Count = Backups.Count;
    }
}