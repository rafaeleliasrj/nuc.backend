using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Features.RefundPayment;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;
using NautiHub.Infrastructure.DataContext;
using NautiHub.Infrastructure.Gateways.Asaas;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Domain.Repositories;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace NautiHub.Application.UseCases.Features.RefundPayment;

/// <summary>
/// Handler para estornar pagamento
/// </summary>
public class RefundPaymentFeatureHandler(
    DatabaseContext context,
    IPaymentRepository paymentRepository,
    IAsaasService asaasService,
    ILogger<RefundPaymentFeatureHandler> logger,
    MessagesService messagesService
) : FeatureHandler(context), IRequestHandler<RefundPaymentFeature, FeatureResponse<RefundPaymentResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IAsaasService _asaasService = asaasService;
    private readonly ILogger<RefundPaymentFeatureHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<FeatureResponse<RefundPaymentResponse>> Handle(RefundPaymentFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar pagamento no banco
            var payment = await _context.Set<Payment>().FindAsync(request.PaymentId);
            if (payment == null)
            {
                _logger.LogWarning("Pagamento {PaymentId} não encontrado", request.PaymentId);
                AddError(_messagesService.Payment_Not_Found);
                return new FeatureResponse<RefundPaymentResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Validar se o pagamento pode ser estornado
            if (!CanRefundPayment(payment, request.Value))
            {
                _logger.LogWarning("Pagamento {PaymentId} não pode ser estornado. Status: {Status}, Valor: {PaymentValue}, Solicitado: {RequestValue}", 
                    request.PaymentId, payment.Status, payment.Value, request.Value);
                AddError(_messagesService.Payment_Refund_Not_Allowed);
                return new FeatureResponse<RefundPaymentResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            // Se não tiver ID do Asaas, retornar erro
            if (string.IsNullOrEmpty(payment.AsaasPaymentId))
            {
                _logger.LogWarning("Pagamento {PaymentId} não possui ID do Asaas", request.PaymentId);
                AddError(_messagesService.Payment_No_Asaas_Id);
                return new FeatureResponse<RefundPaymentResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            // Criar requisição de estorno
            var refundRequest = new AsaasRefundRequest
            {
                Value = request.Value,
                Description = request.Reason
            };

            // Realizar estorno no Asaas
            var refundResult = await _asaasService.RefundPaymentAsync(payment.AsaasPaymentId, refundRequest);
            if (!refundResult.IsSuccess)
            {
                _logger.LogError("Erro ao estornar pagamento no Asaas {PaymentId}: {Error}", 
                    request.PaymentId, refundResult.Error);
                AddError(_messagesService.Payment_Asaas_Error);
                return new FeatureResponse<RefundPaymentResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            var refund = refundResult.Data;

            // Atualizar status do pagamento no banco
            payment.UpdateStatus(PaymentStatus.Refunded);
            await _paymentRepository.UpdateAsync(payment);

            // Montar response
            var response = new RefundPaymentResponse
            {
                PaymentId = payment.Id,
                RefundId = refund.Id,
                RefundedValue = refund.Value,
                RefundDate = refund.DateCreated,
                Status = refund.Status,
                Reason = request.Reason,
                PaymentStatus = payment.Status.ToString()
            };

            _logger.LogInformation("Pagamento {PaymentId} estornado com sucesso. Valor: {Value}", 
                request.PaymentId, refund.Value);

            return new FeatureResponse<RefundPaymentResponse>(ValidationResult, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao estornar pagamento {PaymentId}", request.PaymentId);
            AddError(_messagesService.Payment_General_Error);
            return new FeatureResponse<RefundPaymentResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }

    private static bool CanRefundPayment(Payment payment, decimal refundValue)
    {
        // Só pode estornar pagamentos PAGO
        // O valor do estorno não pode ser maior que o valor do pagamento
        return payment.Status == PaymentStatus.Paid &&
               refundValue > 0 &&
               refundValue <= payment.Value &&
               !string.IsNullOrEmpty(payment.AsaasPaymentId);
    }
}