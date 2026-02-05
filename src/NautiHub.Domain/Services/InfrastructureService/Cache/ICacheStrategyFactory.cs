using NautiHub.Domain.Services.InfrastructureService.Cache.Models.Responses;

namespace NautiHub.Domain.Services.InfrastructureService.Cache;

public interface ICacheStrategyFactory
{
    public ICacheStrategy GetService();
}
