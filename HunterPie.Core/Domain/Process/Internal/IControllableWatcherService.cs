using HunterPie.Core.Domain.Process.Service;

namespace HunterPie.Core.Domain.Process.Internal;

public interface IControllableWatcherService : IProcessWatcherService
{
    public void Start();
}