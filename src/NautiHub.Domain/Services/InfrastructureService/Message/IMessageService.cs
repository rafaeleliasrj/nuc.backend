using NautiHub.Domain.Entities;

namespace NautiHub.Domain.Services
{
    public interface IMessageService
    {
        Task<NotificationResposta> SendConsentAsync(User user, string phone, CancellationToken cancellationToken = default);
        Task<NotificationResposta> SendMessageAsync(string phone, string? body = null, Uri? documentUrl = null, string? documentName = null, CancellationToken cancellationToken = default);
    }

    public record NotificationResposta(bool Success, string? MessageSid = null, string? ErrorMessage = null);
}
