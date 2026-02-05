using NautiHub.Domain.Services.InfrastructureService.Message.Models.Responses;

namespace NautiHub.Domain.Services.InfrastructureService.ServidorDeMensagem;

public interface IMessageStrategyFactory
{
    public IMessageStrategy GestService();
}
