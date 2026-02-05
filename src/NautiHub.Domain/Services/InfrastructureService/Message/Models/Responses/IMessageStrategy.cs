using NautiHub.Domain.Entities;

namespace NautiHub.Domain.Services.InfrastructureService.Message.Models.Responses;

public interface IMessageStrategy
{
    Task<NotificationResposta> SendConsentAsync(User user, string phone, CancellationToken cancellationToken = default);
    Task<NotificationResposta> SendMessageAsync(string phone, string? body = null, Uri? documentUrl = null, string? documentName = null, CancellationToken cancellationToken = default);
}
