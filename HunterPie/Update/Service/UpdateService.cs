using HunterPie.Core.Analytics;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Observability.Logging;
using HunterPie.Features.Analytics.Entity;
using HunterPie.Features.Patches.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.Update.Gateway;
using HunterPie.Update.Presentation;
using HunterPie.Update.Service.Entity;
using HunterPie.Update.UseCase;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Update.Service;

internal class UpdateService : IUpdateUseCase
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private const string JUST_UPDATED_KEY = "JustUpdated";
    private readonly LocalizationUpdateService _localizationUpdateService;
    private readonly UpdateGateway _gateway;
    private readonly ChecksumService _checksumService;
    private readonly ILocalRegistryAsync _localRegistryAsync;
    private readonly IUpdateCleanUpUseCase _updateCleanUpUseCase;
    private readonly IAnalyticsService _analyticsService;
    private readonly IBodyNavigator _bodyNavigator;

    public UpdateService(
        LocalizationUpdateService localizationUpdateService,
        UpdateGateway gateway,
        ChecksumService checksumService,
        ILocalRegistryAsync localRegistryAsync,
        IUpdateCleanUpUseCase updateCleanUpUseCase,
        IAnalyticsService analyticsService,
        IBodyNavigator bodyNavigator)
    {
        _localizationUpdateService = localizationUpdateService;
        _gateway = gateway;
        _checksumService = checksumService;
        _localRegistryAsync = localRegistryAsync;
        _updateCleanUpUseCase = updateCleanUpUseCase;
        _analyticsService = analyticsService;
        _bodyNavigator = bodyNavigator;
    }

    public async Task<bool> InvokeAsync()
    {
        _updateCleanUpUseCase.Invoke();
        await OpenPatchNotesIfNeeded();

        if (!ClientConfig.Config.Client.EnableAutoUpdate)
            return false;

        UpdateViewModel vm = new();
        var view = new UpdateView { DataContext = vm };

        if (!ClientConfig.Config.Client.EnableSeamlessStartup)
            view.Show();

        bool result = await UpdateAsync(vm);

        view.Close();

        return result;
    }

    private async Task OpenPatchNotesIfNeeded()
    {
        bool hasUpdateFlag = await _localRegistryAsync.ExistsAsync(JUST_UPDATED_KEY);
        bool hasJustUpdated = await _localRegistryAsync.GetAsync<bool>(JUST_UPDATED_KEY);

        if (hasUpdateFlag && !hasJustUpdated)
            return;

        _bodyNavigator.Navigate<PatchesViewModel>();
        await _localRegistryAsync.SetAsync(JUST_UPDATED_KEY, false);
    }

    private async Task<bool> UpdateAsync(UpdateViewModel vm)
    {
        await UpdateLocalizationFilesAsync(vm);
        Version? version = await GetLatestVersionAsync(vm);

        if (version is null)
            return false;

        if (ClientConfig.Config.Client.EnableAutoUpdateConfirmation)
        {
            NativeDialogResult result = DialogManager.Warn(
                title: Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='UPDATE_TITLE_STRING']"),
                description: Localization
                    .QueryString("//Strings/Client/Dialogs/Dialog[@Id='UPDATE_CONFIRMATION_DESCRIPTION_STRING']")
                    .Replace("{Latest}", $"{version}"),
                buttons: NativeDialogButtons.Accept | NativeDialogButtons.Reject
            );

            if (result != NativeDialogResult.Accept)
                return false;
        }

        string? packageFile = await DownloadPackageAsync(vm, version);

        if (packageFile is null)
            return false;

        string? extractedPackagePath = ExtractPackage(vm, packageFile);

        if (extractedPackagePath is null)
            return false;

        UpdateFileChecksums? checksums = await CalculateChecksumsAsync(vm, extractedPackagePath);

        if (checksums is null)
            return false;

        try
        {
            ReplaceFiles(vm, extractedPackagePath, checksums);
            CleanUp(vm, extractedPackagePath, packageFile);
            await _localRegistryAsync.SetAsync("JustUpdated", true);
            return true;
        }
        catch (Exception err)
        {
            await _analyticsService.SendAsync(
                analyticsEvent: AnalyticsEvent.FromException(err, false)
            );

            string dialogMessage = err switch
            {
                IOException => "Failed to replace old files, make sure HunterPie has permissions to move files.",
                UnauthorizedAccessException => "HunterPie is missing permissions to manage files.",
                _ => "Failed to update HunterPie. Please check it is in a non-special folder and that it has permissions to write to files.",
            };

            DialogManager.Error(
                title: "Update failed",
                description: dialogMessage,
                buttons: NativeDialogButtons.Accept
            );

            _logger.Error($"Failed to update HunterPie: {err}");

            return false;
        }
    }

    private async Task UpdateLocalizationFilesAsync(UpdateViewModel vm)
    {
        vm.State = "Checking for new localization files...";
        await _localizationUpdateService.InvokeAsync();
    }

    private async Task<Version?> GetLatestVersionAsync(UpdateViewModel vm)
    {
        vm.State = "Checking for latest version";
        string? latest = await _gateway.GetLatestVersionAsync();

        if (latest is null)
            return null;

        var latestVersion = new Version(latest);

        if (ClientInfo.IsVersionGreaterOrEq(latestVersion))
            return null;

        vm.State = $"Found version {latestVersion}";

        return latestVersion;
    }

    private async Task<string?> DownloadPackageAsync(UpdateViewModel vm, Version version)
    {
        string packagePath = ClientInfo.GetRandomTempFile();

        vm.State = "Downloading package...";

        bool success = await _gateway.DownloadVersionAsync(
            version: version.ToString(),
            output: packagePath,
            callback: (_, args) =>
            {
                vm.DownloadedBytes = args.BytesDownloaded;
                vm.TotalBytes = args.TotalBytes;
            });

        if (success)
            return packagePath;

        vm.State = "Failed to update HunterPie";
        _logger.Warning("Failed to update HunterPie");

        return null;
    }

    private string? ExtractPackage(UpdateViewModel vm, string packagePath)
    {
        string packageDirectory = ClientInfo.GetRandomTempDirectory();

        vm.State = "Extracting package...";
        try
        {
            ZipFile.ExtractToDirectory(
                sourceArchiveFileName: packagePath,
                destinationDirectoryName: packageDirectory
            );
            File.Delete(packagePath);

            return packageDirectory;
        }
        catch (Exception err)
        {
            _logger.Error($"Failed to update HunterPie: {err}");

            return null;
        }
    }

    private async Task<UpdateFileChecksums?> CalculateChecksumsAsync(UpdateViewModel vm, string packageFolder)
    {
        vm.State = "Calculating file hashes...";

        try
        {
            Dictionary<string, string> remote = await _checksumService.IndexAsync(
                basePath: packageFolder,
                relativePath: string.Empty
            );
            Dictionary<string, string> local = await _checksumService.IndexAsync(
                basePath: ClientInfo.ClientPath,
                relativePath: string.Empty,
                ignorableFiles: new HashSet<string>
                {
                    ClientInfo.GetPathFor("HunterPie_Log.txt"),
                    ClientInfo.GetPathFor("config.json")
                }
            );

            return new UpdateFileChecksums(
                Remote: remote,
                Local: local
                    .Where(it => remote.ContainsKey(it.Key))
                    .ToDictionary()
            );
        }
        catch (Exception err)
        {
            _logger.Error($"Failed to update HunterPie: {err}");
            return null;
        }
    }

    private void ReplaceFiles(UpdateViewModel vm, string packagePath, UpdateFileChecksums checksums)
    {
        vm.State = "Replacing old files...";

        IEnumerable<string> updateFiles = checksums.Remote
            .Where(it => !checksums.Local.ContainsKey(it.Key) || checksums.Local[it.Key] != it.Value)
            .Select(it => it.Key);

        foreach (string relativeFilePath in updateFiles)
        {
            string oldFile = ClientInfo.GetPathFor(relativeFilePath + ".old");
            string localFile = ClientInfo.GetPathFor(relativeFilePath);
            string latestFile = Path.Combine(packagePath, relativeFilePath);

            if (File.Exists(oldFile))
                File.Delete(oldFile);

            if (File.Exists(localFile))
                File.Move(
                    sourceFileName: localFile,
                    destFileName: oldFile
                );
            else if (Path.GetDirectoryName(localFile) is { } directory)
                Directory.CreateDirectory(directory);

            File.Move(
                sourceFileName: latestFile,
                destFileName: localFile
            );
        }
    }

    private void CleanUp(UpdateViewModel vm, string packagePath, string packageFile)
    {
        vm.State = "Cleaning up temporary files...";

        File.Delete(packageFile);
        Directory.Delete(packagePath, true);
    }
}