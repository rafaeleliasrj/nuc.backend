using Microsoft.Extensions.DependencyInjection;
using NautiHub.Domain.Services.InfrastructureService.Message.Models.Responses;
using NautiHub.Domain.Services.InfrastructureService.ServidorDeMensagem;
using NautiHub.Infrastructure.Services.Message.Enums;
using NautiHub.Infrastructure.Services.Message.Servers.Meta;
using NautiHub.Infrastructure.Services.Message.Servers.Twilio;

namespace NautiHub.Infrastructure.Services.Message;

public class MessageStrategyFactory : IMessageStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MessageEnum _server;

    public MessageStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var servidorEnv = Environment.GetEnvironmentVariable("SERVIDOR_DE_MENSAGEM");
        _server = Enum.TryParse(servidorEnv, out MessageEnum servidor)
            ? servidor
            : MessageEnum.Meta;
    }

    public IMessageStrategy GestService()
    {
        return _server switch
        {
            MessageEnum.Twilio => _serviceProvider.GetRequiredService<TwilioStrategy>(),
            MessageEnum.Meta => _serviceProvider.GetRequiredService<MetaStrategy>(),
            _ => _serviceProvider.GetRequiredService<TwilioStrategy>()
        };
    }
}
