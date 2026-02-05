using Microsoft.Extensions.Logging;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Services;
using NautiHub.Domain.Services.InfrastructureService.Message.Models.Responses;
using NautiHub.Domain.Services.InfrastructureService.ServidorDeMensagem;

namespace NautiHub.Infrastructure.Services.Message;

public class MessageService : IMessageService
{
    private readonly ILogger<MessageService> _logger;
    private readonly IMessageStrategyFactory _strategyFactory;

    public MessageService(ILogger<MessageService> logger,
                                    IMessageStrategyFactory strategyFactory)
    {
        _logger = logger;
        _strategyFactory = strategyFactory;
    }

    public async Task<NotificationResposta> SendConsentAsync(User user, string phone, CancellationToken cancellationToken = default)
    {
        IMessageStrategy strategy = _strategyFactory.GestService();
        return await strategy.SendConsentAsync(user, phone, cancellationToken);
    }

    public async Task<NotificationResposta> SendMessageAsync(string phone, string? body = null, Uri? documentUrl = null, string? documentName = null, CancellationToken cancellationToken = default)
    {
        IMessageStrategy strategy = _strategyFactory.GestService();
        return await strategy.SendMessageAsync(phone, body, documentUrl, documentName, cancellationToken);
    }
}
