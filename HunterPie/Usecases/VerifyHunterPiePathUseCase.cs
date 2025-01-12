using HunterPie.Core.Client;
using HunterPie.Core.Extensions;

namespace HunterPie.Usecases;
internal class VerifyHunterPiePathUseCase
{

    public static bool Invoke() => !ClientInfo.ClientPath.ToLowerInvariant()
        .ContainsAny(new[] { "temp", "appdata" });

}