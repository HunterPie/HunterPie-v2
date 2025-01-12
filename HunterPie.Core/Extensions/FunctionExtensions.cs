using System;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Extensions;

public static class FunctionExtensions
{
    public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
    {
        int last = 0;

        void Action(T param)
        {
            int current = Interlocked.Increment(ref last);
            _ = Task.Delay(milliseconds)
                .ContinueWith(task =>
                {
                    if (current == last)
                        func(param);

                    task.Dispose();
                });
        }

        return Action;
    }
}