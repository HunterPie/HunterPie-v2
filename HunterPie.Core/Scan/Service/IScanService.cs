using HunterPie.Core.Architecture;
using HunterPie.Core.Domain;

namespace HunterPie.Core.Scan.Service;

public interface IScanService
{
    public Observable<long> ElapsedTime { get; }

    public void Add(Scannable scannable);

    public void Remove(Scannable scannable);
}