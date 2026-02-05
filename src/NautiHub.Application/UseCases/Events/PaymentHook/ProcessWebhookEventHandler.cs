using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Core.MessageEvents;
using NautiHub.Core.Mediator;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;
using NautiHub.Infrastructure.Repositories;

namespace NautiHub.Application.UseCases.Events.PaymentHook;

/// <summary>
/// Handler responsável por executar evento de webhook do Asaas.
/// </summary>
/// <remarks>
/// Create do handler de evento.
/// </remarks>
/// <param name="serviceScopeFactory">Factory para execução paralela.</param>
/// <param name="logger">Logs</param>
public class ProcessWebhookEventHandler(IServiceScopeFactory serviceScopeFactory,
                                 ILogger<IEventHandler<ProcessWebhookEvent>> logger) : IEventHandler<ProcessWebhookEvent>
{
    public async Task OnExecuteConsume(ProcessWebhookEvent context)
    {
        using (IServiceScope scope = serviceScopeFactory.CreateScope())
        {
            IMediatorHandler mediator = scope.ServiceProvider.GetService<IMediatorHandler>();
            var logger = scope.ServiceProvider.GetService<ILogger<ProcessWebhookEventHandler>>();
            var paymentRepository = scope.ServiceProvider.GetService<IPaymentRepository>();
            var bookingRepository = scope.ServiceProvider.GetService<IBookingRepository>();

            try
            {
                var webhookEvent = context.WebhookEvent;
                
                logger.LogInformation("Processando webhook: {Event} para pagamento {PaymentId}", 
                    webhookEvent.Event, webhookEvent.Payment?.Id);

                // Se não houver dados de pagamento, ignorar evento
                if (webhookEvent.Payment == null)
                {
                    logger.LogWarning("Webhook sem dados de pagamento: {Event}", webhookEvent.Event);
                    return;
                }

                // Buscar pagamento no banco
                var payment = await paymentRepository.GetByAsaasPaymentIdAsync(webhookEvent.Payment.Id);
                if (payment == null)
                {
                    logger.LogWarning("Pagamento {AsaasPaymentId} não encontrado", webhookEvent.Payment.Id);
                    return;
                }

                // Mapear evento do Asaas para nosso status
                var newStatus = MapAsaasEventToPaymentStatus(webhookEvent.Event);
                
                // Atualizar status do pagamento
                payment.UpdateStatus(newStatus);
                
                // Se for status de pagamento confirmado/pago, atualizar reserva associada
                if (newStatus == PaymentStatus.Paid)
                {
                    await UpdateBookingStatusAsync(payment, bookingRepository);
                }

                await paymentRepository.UpdateAsync(payment);

                logger.LogInformation("Webhook processado com sucesso: {Event} -> {Status}", 
                    webhookEvent.Event, newStatus);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao processar webhook do Asaas");
                throw;
            }
        }
    }

    private PaymentStatus MapAsaasEventToPaymentStatus(string asaasEvent)
    {
        return asaasEvent?.ToUpperInvariant() switch
        {
            "PAYMENT_CREATED" => PaymentStatus.Pending,
            "PAYMENT_CONFIRMED" => PaymentStatus.Paid,
            "PAYMENT_RECEIVED" => PaymentStatus.Paid,
            "PAYMENT_OVERDUE" => PaymentStatus.Failed,
            "PAYMENT_DELETED" => PaymentStatus.Failed,
            "PAYMENT_REFUNDED" => PaymentStatus.Refunded,
            "PAYMENT_CHARGEBACK_REQUESTED" => PaymentStatus.Failed,
            "PAYMENT_SPLIT_APPROVED" => PaymentStatus.Paid,
            "PAYMENT_SPLIT_REFUSED" => PaymentStatus.Failed,
            "INVOICE_DELETED" => PaymentStatus.Failed,
            _ => PaymentStatus.Pending
        };
    }

    private async Task UpdateBookingStatusAsync(Domain.Entities.Payment payment, IBookingRepository bookingRepository)
    {
        var booking = await bookingRepository.GetByIdAsync(payment.BookingId);
        if (booking != null)
        {
            booking.MarkAsPaid(payment.BillingType, payment.AsaasPaymentId);
            await bookingRepository.UpdateAsync(booking);
        }
    }
}