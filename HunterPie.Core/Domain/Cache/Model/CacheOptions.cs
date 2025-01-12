using System;

namespace HunterPie.Core.Domain.Cache.Model;

public record CacheOptions(
    TimeSpan Ttl
)
{
    public static readonly CacheOptions Default = new(Ttl: TimeSpan.MaxValue);
}