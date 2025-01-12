using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Logger;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Backup.Strategies;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.Integrations.Poogie.Backup.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameType = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.Features.Backup.Services;

internal class GameSaveBackupService : IBackupService
{
    private readonly IAccountUseCase _accountUseCase;
    private readonly AccountConfig _accountConfig;
    private readonly PoogieBackupConnector _connector;
    private readonly IBackupStrategy[] _strategies;
    private readonly ILocalRegistryAsync _localRegistryAsync;

    private const string LAST_BACKUP_AT_KEY = "lastBackupAt:";
    private const string WAS_LAST_BACKUP_SUCCESS_KEY = "wasLastBackupSuccessful:";

    public GameSaveBackupService(
        IAccountUseCase accountUseCase,
        AccountConfig accountConfig,
        PoogieBackupConnector connector,
        IBackupStrategy[] strategies,
        ILocalRegistryAsync localRegistryAsync)
    {
        _accountUseCase = accountUseCase;
        _accountConfig = accountConfig;
        _connector = connector;
        _strategies = strategies;
        _localRegistryAsync = localRegistryAsync;
    }

    public async Task ExecuteAsync(GameType gameType)
    {
        bool isLoggedIn = await _accountUseCase.IsValidSessionAsync();

        if (!isLoggedIn)
            return;

        if (!_accountConfig.IsBackupEnabled)
            return;

        PoogieResult<CanUploadBackupResponse> canUploadResult = await _connector.CanUploadBackupAsync();

        if (canUploadResult.Response is not { CanUpload: true })
            return;

        string registryKey = $"{LAST_BACKUP_AT_KEY}{gameType}";
        string successRegistryKey = $"{WAS_LAST_BACKUP_SUCCESS_KEY}{gameType}";

        if (!await CanBackupFilesAsync(gameType))
            return;

        IBackupStrategy? strategy = _strategies.FirstOrDefault(strategy => strategy.Type == gameType.ToApiModel());

        if (strategy is not { })
            throw new Exception($"{gameType} is not supported for backups");

        string steamPath = GetSteamSaveFolder() ??
                           throw new DirectoryNotFoundException(
                               "Steam userdata folder not found. Could not finish backup of save files");

        string backupFile = await strategy.PackFilesAsync(steamPath);

        PoogieResult<BackupResponse> result = await _connector.UploadAsync(strategy.Type, backupFile);

        if (File.Exists(backupFile))
            try
            {
                File.Delete(backupFile);
            }
            catch
            {
                // ignored
            }

        if (result.Response is null)
            return;

        Log.Debug("Successfully uploaded save file {0}", result.Response!.Id);

        await _localRegistryAsync.SetAsync(registryKey, DateTime.UtcNow.Ticks);
        await _localRegistryAsync.SetAsync(successRegistryKey, true);
    }

    private async Task<bool> CanBackupFilesAsync(GameType gameType)
    {
        string registryKey = $"{LAST_BACKUP_AT_KEY}{gameType}";

        if (!await _localRegistryAsync.ExistsAsync(registryKey))
            return true;

        long lastBackupTicks = await _localRegistryAsync.GetAsync<long>(registryKey);

        double timeSinceLastBackup = (DateTime.UtcNow - new DateTime(lastBackupTicks)).TotalHours;

        return timeSinceLastBackup >= 23;
    }

    private static string? GetSteamSaveFolder()
    {
        using RegistryKey? activeProcess = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam\ActiveProcess");

        if (activeProcess is null)
            return null;

        int? activeUser = (int?)activeProcess.GetValue("ActiveUser");
        string? steamClientPath = (string?)activeProcess.GetValue("SteamClientDll");

        if (activeUser is null || steamClientPath is null)
            return null;

        string steamPath = Path.GetDirectoryName(steamClientPath)!;

        return Path.Combine(steamPath, "userdata", activeUser.Value.ToString());
    }
}