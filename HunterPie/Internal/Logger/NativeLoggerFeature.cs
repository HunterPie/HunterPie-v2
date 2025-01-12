using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Core.Logger;

namespace HunterPie.Internal.Logger;

internal class NativeLoggerFeature : Feature
{
    protected override void OnEnable() => Log.Debug("Enabled feature");

    protected override void OnDisable() => Log.Debug("Disabled feature");
}