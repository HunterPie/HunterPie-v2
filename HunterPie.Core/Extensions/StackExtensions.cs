using System.Collections.Generic;

namespace HunterPie.Core.Extensions;

#nullable enable
public static class StackExtensions
{
    public static T? PopOrDefault<T>(this Stack<T> stack) where T : class =>
        stack.TryPop(out T? result) ? result : default;

    public static void PushNotNull<T>(this Stack<T> stack, T? value) where T : class
    {
        if (value is null)
            return;

        stack.Push(value);
    }
}