using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NautiHub.Domain.Services.InfrastructureService.Cache;
using NautiHub.Domain.Services.InfrastructureService.Cache.Models.Responses;

namespace NautiHub.Infrastructure.Services.Cache;

public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private readonly ICacheStrategyFactory _strategyFactory;

    public CacheService(IConfiguration configuration,
                          ILogger<CacheService> logger,
                          ICacheStrategyFactory strategyFactory)
    {
        _logger = logger;
        _strategyFactory = strategyFactory;
    }

    public async Task<T?> GetAsync<T>(string cacheKey)
    {
        ICacheStrategy strategy = _strategyFactory.GetService();
        return await strategy.GetCache<T>(cacheKey);
    }

    public async Task SaveAsync<T>(string cacheKey, T objeto, int timerBufferSegundos)
    {
        ICacheStrategy strategy = _strategyFactory.GetService();
        await strategy.SaveToCache(cacheKey, objeto, timerBufferSegundos);
    }

    public async Task DeleteAsync(string cacheKey)
    {
        ICacheStrategy strategy = _strategyFactory.GetService();
        await strategy.RemoveCache(cacheKey);
    }
}
