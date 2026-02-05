using Microsoft.Extensions.DependencyInjection;
using NautiHub.CrossCutting.Services.Cache.Enums;
using NautiHub.CrossCutting.Services.Cache.Operators.MemoryCache;
using NautiHub.CrossCutting.Services.Cache.Operators.Redis;
using NautiHub.Domain.Services.InfrastructureService.Cache;
using NautiHub.Domain.Services.InfrastructureService.Cache.Models.Responses;

namespace NautiHub.CrossCutting.Services.Cache;

public class CacheStrategyFactory : ICacheStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CacheEnum _servidor;

    public CacheStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var servidorEnv = Environment.GetEnvironmentVariable("SERVIDOR_CACHE") ?? "MemoryCache";
        _servidor = Enum.TryParse(servidorEnv, out CacheEnum servidor)
            ? servidor
            : CacheEnum.MemoryCache;
    }

    public ICacheStrategy GetService()
    {
        return _servidor switch
        {
            CacheEnum.Redis => _serviceProvider.GetRequiredService<RedisCacheStrategy>(),
            _ => _serviceProvider.GetRequiredService<MemoryCacheStrategy>(),
        };
    }
}
