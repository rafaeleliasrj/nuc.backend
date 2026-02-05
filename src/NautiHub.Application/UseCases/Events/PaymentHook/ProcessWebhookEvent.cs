using NautiHub.Core.MessageEvents;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;

namespace NautiHub.Application.UseCases.Events.PaymentHook;

/// <remarks>
/// Evento responsável por processar webhook do Asaas.
/// </remarks>
public class ProcessWebhookEvent(AsaasWebhookEvent webhookEvent) : Event
{
    /// <summary>
    /// Evento recebido do webhook
    /// </summary>
    public AsaasWebhookEvent WebhookEvent { get; init; } = webhookEvent;
}