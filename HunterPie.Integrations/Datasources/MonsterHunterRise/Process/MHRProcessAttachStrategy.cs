using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Process;

internal class MHRProcessAttachStrategy : IProcessAttachStrategy
{
    public string Name => SupportedGameNames.MONSTER_HUNTER_RISE;

    public GameProcessType Game => GameProcessType.MonsterHunterRise;

    private ProcessStatus _status;

    public ProcessStatus Status
    {
        get => _status;
        private set
        {
            if (value == _status)
                return;

            ProcessStatus oldStatus = _status;
            _status = value;

            this.Dispatch(
                StatusChange,
                new SimpleValueChangeEventArgs<ProcessStatus>(oldStatus, value)
            );
        }
    }

    public event EventHandler<SimpleValueChangeEventArgs<ProcessStatus>>? StatusChange;

    public bool CanAttach(SystemProcess process)
    {
        if (!process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER RISE"))
            return false;

        string? version = process.MainModule?.FileVersionInfo.FileVersion;

        if (version is not { })
            throw new UnauthorizedAccessException("Failed to get Monster Hunter Rise version, missing permissions");

        bool hasLoaded = AddressMap.Parse(
            filePath: Path.Combine(ClientInfo.AddressPath, $"MonsterHunterRise.{version}.map")
        );

        if (!hasLoaded)
            throw new Exception($"Failed to load address for Monster Hunter Rise v{version}");

        return true;
    }

    public void SetStatus(ProcessStatus status) => Status = status;
}