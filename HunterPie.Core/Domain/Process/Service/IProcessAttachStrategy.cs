using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Core.Domain.Process.Service;

public interface IProcessAttachStrategy : IGameWatcher
{
    public bool CanAttach(SystemProcess process);

    public void SetStatus(ProcessStatus status);
}