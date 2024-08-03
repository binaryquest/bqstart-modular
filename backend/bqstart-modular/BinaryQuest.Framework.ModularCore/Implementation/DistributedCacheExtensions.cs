using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BinaryQuest.Framework.ModularCore.Implementation;

public static class DistributedCacheExtensions
{
    private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReferenceHandler = ReferenceHandler.Preserve
    };

    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
    {
        return SetAsync(cache, key, value, new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
    }

    public static Task Set<T>(this IDistributedCache cache, string key, T value)
    {
        return Set(cache, key, value, new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
    }

    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, serializerOptions));
        return cache.SetAsync(key, bytes, options);
    }

    public static Task Set<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, serializerOptions));
        return cache.SetAsync(key, bytes, options);
    }

    public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
    {
        var val = cache.Get(key);
        value = default;
        if (val == null) return false;        

        value = JsonSerializer.Deserialize<T>(val, serializerOptions);
        return true;
    }

    public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> task, DistributedCacheEntryOptions? options = null)
    {
#if DEBUG
        return await task();
#else
        options ??= new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
        if (cache.TryGetValue(key, out T? value) && value is not null)
        {
            return value;
        }
        value = await task();
        if (value is not null)
        {
            await cache.SetAsync<T>(key, value, options);
        }
        return value;
#endif        
    }

    public static T? GetOrSet<T>(this IDistributedCache cache, string key, Func<T> task, DistributedCacheEntryOptions? options = null)
    {
        options ??= new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
        if (cache.TryGetValue(key, out T? value) && value is not null)
        {
            return value;
        }
        value = task();
        if (value is not null)
        {
            cache.Set<T>(key, value, options);
        }
        return value;
    }
}