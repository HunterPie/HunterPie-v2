using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Backup.Services;
using HunterPie.Features.Backup.Strategies;
using HunterPie.Features.Backup.ViewModels;

namespace HunterPie.Features.Backup;

internal class BackupModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<MHRBackupStrategy>()
            .WithService<MHWBackupStrategy>()
            .WithSingle<GameSaveBackupService>()
            .WithService<BackupElementFactory>();

        registry
            .WithService<BackupsViewModel>();
    }
}