using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Utils;

public static class AsyncHelper
{
    private static readonly TaskFactory TaskFactory = new TaskFactory(
        cancellationToken: CancellationToken.None,
        creationOptions: TaskCreationOptions.None,
        continuationOptions: TaskContinuationOptions.None,
        scheduler: TaskScheduler.Default
    );

    public static T RunSync<T>(Func<Task<T>> asyncDelegate)
    {
        return TaskFactory
            .StartNew(asyncDelegate)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    public static void RunSync(Func<Task> func)
    {
        TaskFactory
            .StartNew<Task>(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    public static async Task<IEnumerable<T>> AwaitAll<T>(this IEnumerable<Task<T>> self)
    {
        return await Task.WhenAll(self);
    }

    public static List<T> Collect<T>(this IAsyncEnumerable<T> self)
    {
        return self.ToBlockingEnumerable()
            .ToList();
    }

    public static IEnumerable<T> AwaitResults<T>(this IEnumerable<Task<T>> source) =>
        source.Select(it => it.Result);
}