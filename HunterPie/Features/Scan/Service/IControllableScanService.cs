using HunterPie.Core.Scan.Service;
using System.Threading;

namespace HunterPie.Features.Scan.Service;

internal interface IControllableScanService : IScanService
{
    public void Start(CancellationToken cancellationToken);
}