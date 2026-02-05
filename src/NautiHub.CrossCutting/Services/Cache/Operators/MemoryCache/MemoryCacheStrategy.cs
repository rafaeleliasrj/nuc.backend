using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NautiHub.Domain.Services.InfrastructureService.Cache.Models.Responses;
using NautiHub.Infrastructure.Services.Cache;

namespace NautiHub.CrossCutting.Services.Cache.Operators.MemoryCache;

public class MemoryCacheStrategy : ICacheStrategy
{
    private readonly ILogger<CacheService> _logger;
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheStrategy(ILogger<CacheService> logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<T?> GetCache<T>(string cacheKey)
    {
        if (_memoryCache.TryGetValue(cacheKey, out T? cachedData))
            return await Task.FromResult(cachedData);

        return default;
    }

    public async Task SaveToCache<T>(string cacheKey, T objeto, int timerBufferSegundos)
    {
        if (_memoryCache.TryGetValue(cacheKey, out T? cachedData))
            _memoryCache.Remove(cacheKey);

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(timerBufferSegundos),
            SlidingExpiration = TimeSpan.FromSeconds(timerBufferSegundos),
        };

        _memoryCache.Set(cacheKey, objeto, cacheOptions);

        await Task.CompletedTask;
        return;
    }

    public async Task RemoveCache(string cacheKey)
    {
        _memoryCache.Remove(cacheKey);

        await Task.CompletedTask;
    }
}
