using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Infrastructure;

public static class CachingExtensions
{
    public static void Set<T>(this IDistributedCache cache, string key, T value,
        DistributedCacheEntryOptions options) => cache.Set(key, JsonSerializer.SerializeToUtf8Bytes(value), options);
    
    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value,
        DistributedCacheEntryOptions options, CancellationToken token = default) =>
        await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), options, token);

    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key,
        CancellationToken token = default)
    {
        var bytes = await cache.GetAsync(key, token);
        return bytes is null ? default : JsonSerializer.Deserialize<T>(bytes);
    }
}