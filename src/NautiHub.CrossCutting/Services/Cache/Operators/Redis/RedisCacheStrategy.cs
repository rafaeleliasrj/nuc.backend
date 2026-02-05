using Microsoft.Extensions.Logging;
using NautiHub.Domain.Services.InfrastructureService.Cache.Models.Responses;
using NautiHub.Infrastructure.Services.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace NautiHub.CrossCutting.Services.Cache.Operators.Redis;

public class RedisCacheStrategy : ICacheStrategy
{
    private readonly ILogger<CacheService> _logger;
    private readonly IConnectionMultiplexer? _redisMultiplexer;

    public RedisCacheStrategy(ILogger<CacheService> logger, IConnectionMultiplexer? redisMultiplexer = null)
    {
        _logger = logger;
        _redisMultiplexer = redisMultiplexer;
    }

    public async Task<T?> GetCache<T>(string cacheKey)
    {
        try
        {
            if (_redisMultiplexer == null)
            {
                _logger.LogWarning("[GetCache] - Redis não está configurado. Retornando valor padrão.");
                return default;
            }

            IDatabase db = _redisMultiplexer.GetDatabase();
            RedisValue cachedData = await db.StringGetAsync(cacheKey);

            if (!cachedData.IsNullOrEmpty)
                return JsonSerializer.Deserialize<T>(cachedData!.ToString());
        }
        catch (Exception)
        {
            _logger.LogInformation("[GetCache] - [cacheKey: {cacheKey}] - Ocorreu erro ao obter cache.", cacheKey);
        }

        return default;
    }

    public async Task SaveToCache<T>(string cacheKey, T objeto, int timerBufferSegundos)
    {
        try
        {
            if (_redisMultiplexer == null)
            {
                _logger.LogWarning("[SaveToCache] - Redis não está configurado. Operação ignorada.");
                return;
            }

            IDatabase db = _redisMultiplexer.GetDatabase();
            var serializedData = System.Text.Json.JsonSerializer.Serialize(objeto);

            await db.StringSetAsync(cacheKey, serializedData, TimeSpan.FromSeconds(timerBufferSegundos));
        }
        catch (Exception)
        {
            _logger.LogInformation("[SaveToCache] - [cacheKey: {cacheKey}] - Ocorreu erro ao salvar em cache.", cacheKey);
        }
    }

    public async Task RemoveCache(string cacheKey)
    {
        try
        {
            if (_redisMultiplexer == null)
            {
                _logger.LogWarning("[RemoveCache] - Redis não está configurado. Operação ignorada.");
                return;
            }

            IDatabase db = _redisMultiplexer.GetDatabase();
            await db.KeyDeleteAsync(cacheKey);
        }
        catch (Exception)
        {
            _logger.LogInformation("[RemoveCache] - [cacheKey: {cacheKey}] - Ocorreu erro ao remover cache.", cacheKey);
        }
    }
}
