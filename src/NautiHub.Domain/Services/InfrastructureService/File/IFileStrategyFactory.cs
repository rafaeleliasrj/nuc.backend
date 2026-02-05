using NautiHub.Domain.Services.InfrastructureService.File.Models.Responses;

namespace NautiHub.Domain.Services.InfrastructureService.File;

public interface IFileStrategyFactory
{
    public IFileStrategy GetService();
}
