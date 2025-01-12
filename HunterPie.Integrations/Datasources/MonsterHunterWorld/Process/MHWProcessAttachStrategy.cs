using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Process;

internal class MHWProcessAttachStrategy : IProcessAttachStrategy
{
    public string Name => SupportedGameNames.MONSTER_HUNTER_WORLD;

    public GameProcessType Game => GameProcessType.MonsterHunterWorld;

    private ProcessStatus _status;

    public ProcessStatus Status
    {
        get => _status;
        private set
        {
            if (_status == value)
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
        if (!process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER: WORLD"))
            return false;

        string? rawVersion = process.MainWindowTitle.Split('(')
            .ElementAtOrDefault(1)
            ?.TrimEnd(')');
        bool isValidVersion = int.TryParse(rawVersion, out int version);

        if (!isValidVersion)
            throw new UnauthorizedAccessException("Failed to get Monster Hunter World version");

        bool hasLoaded = IsIce(version)
            ? AddressMap.ParseLatest(ClientInfo.AddressPath)
            : AddressMap.Parse(Path.Combine(ClientInfo.AddressPath, $"MonsterHunterWorld.{rawVersion}.map"));

        if (!hasLoaded)
            throw new Exception($"Failed to load address for Monster Hunter World v{version}");

        return true;
    }

    public void SetStatus(ProcessStatus status) => Status = status;

    private static bool IsIce(int version) => version is >= 300_000 and <= 400_000;
}