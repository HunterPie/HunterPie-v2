using System;

namespace HunterPie.Core.Domain.Cache.Model;

internal record ThreadSafeCacheEntry(
    DateTime ExpiresAt,
    object Value
);