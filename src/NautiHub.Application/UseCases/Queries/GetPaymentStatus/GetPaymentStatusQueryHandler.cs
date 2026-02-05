using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Messages.Queries;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Domain.Enums;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Infrastructure.Gateways.Asaas;
using NautiHub.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace NautiHub.Application.UseCases.Queries.GetPaymentStatus;

/// <summary>
/// Handler para obter status de pagamento
/// </summary>
public class GetPaymentStatusQueryHandler(
    DatabaseContext context,
    IAsaasService asaasService,
    ILogger<GetPaymentStatusQueryHandler> logger,
    MessagesService messagesService
) : QueryHandler
{
    private readonly DatabaseContext _context = context;
    private readonly IAsaasService _asaasService = asaasService;
    private readonly ILogger<GetPaymentStatusQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<GetPaymentStatusResponse>> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar pagamento no banco
            var payment = await _context.Set<Payment>().FindAsync(request.Id);
            if (payment == null)
            {
                AddError(_messagesService.Payment_Not_Found);
                return new QueryResponse<GetPaymentStatusResponse>(ValidationResult);
            }

            // Se tiver ID do Asaas, buscar status atualizado
            var currentStatus = payment.Status;
            if (!string.IsNullOrEmpty(payment.AsaasPaymentId))
            {
                var asaasResult = await _asaasService.GetPaymentAsync(payment.AsaasPaymentId);
                if (asaasResult.IsSuccess)
                {
                    // Mapear status do Asaas para nosso enum
                    currentStatus = MapAsaasStatusToPaymentStatus(asaasResult.Data.Status);
                }
            }

            // Montar response
            var response = new GetPaymentStatusResponse
            {
                Id = payment.Id,
                Status = currentStatus.ToString(),
                StatusDescription = GetStatusDescription(currentStatus),
                LastUpdated = GetLastUpdatedDate(payment),
                CanCancel = CanCancelPayment(currentStatus),
                CanRefund = CanRefundPayment(currentStatus)
            };

            return new QueryResponse<GetPaymentStatusResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter status do pagamento {PaymentId}", request.Id);
            AddError(_messagesService.Payment_Get_Status_Error);
            return new QueryResponse<GetPaymentStatusResponse>(ValidationResult);
        }
    }

    private static PaymentStatus MapAsaasStatusToPaymentStatus(string asaasStatus)
    {
        return asaasStatus?.ToUpperInvariant() switch
        {
            "PENDING" or "AWAITING_PAYMENT" => PaymentStatus.Pending,
            "CONFIRMED" or "RECEIVED" => PaymentStatus.Paid,
            "OVERDUE" or "DECLINED" or "CANCELED" => PaymentStatus.Failed,
            "REFUNDED" => PaymentStatus.Refunded,
            _ => PaymentStatus.Pending
        };
    }

    private string GetStatusDescription(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Pending => "Aguardando pagamento",
            PaymentStatus.Paid => "Pago",
            PaymentStatus.Failed => "Falhou",
            PaymentStatus.Refunded => "Estornado",
            _ => "Desconhecido"
        };
    }

    private static DateTime GetLastUpdatedDate(Payment payment)
    {
        return payment.ConfirmedDate ?? payment.CreatedAt ?? DateTime.Now;
    }

    private static bool CanCancelPayment(PaymentStatus status)
    {
        return status == PaymentStatus.Pending;
    }

    private static bool CanRefundPayment(PaymentStatus status)
    {
        return status == PaymentStatus.Paid;
    }
}