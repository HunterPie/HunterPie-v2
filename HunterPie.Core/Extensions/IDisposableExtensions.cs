using System;

namespace HunterPie.Core.Extensions;

public static class IDisposableExtensions
{

    public static void DisposeAll(params IDisposable[] disposables)
    {
        foreach (IDisposable disposable in disposables)
            disposable.Dispose();
    }
}