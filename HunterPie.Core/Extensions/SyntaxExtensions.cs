using System;

namespace HunterPie.Core.Extensions;

#nullable enable
public static class SyntaxExtensions
{
    public static T? Also<T>(this T? self, Action<T> block)
    {
        if (self is not null)
            block(self);

        return self;
    }

    public static TOut? Let<TIn, TOut>(this TIn? self, Func<TIn, TOut> block)
    {
        return self is not null ? block(self) : default;
    }
}