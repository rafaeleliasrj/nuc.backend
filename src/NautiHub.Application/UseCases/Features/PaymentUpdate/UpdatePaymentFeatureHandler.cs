using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using System.Net;
using MediatR;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Application.UseCases.Features.PaymentUpdate;

/// <summary>
/// Handler para atualização de pagamento
/// </summary>
public class UpdatePaymentFeatureHandler : FeatureHandler, IRequestHandler<UpdatePaymentFeature, FeatureResponse<PaymentResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<UpdatePaymentFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public UpdatePaymentFeatureHandler(
        DatabaseContext context,
        IPaymentRepository paymentRepository,
        ILogger<UpdatePaymentFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _paymentRepository = paymentRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<PaymentResponse>> Handle(UpdatePaymentFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar pagamento
            var payment = await _context.Set<Payment>().FindAsync(request.PaymentId);
            if (payment == null)
            {
                _logger.LogWarning("Pagamento {PaymentId} não encontrado", request.PaymentId);
                AddError(_messagesService.Payment_Not_Found);
                return new FeatureResponse<PaymentResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Atualmente a entidade Payment não possui métodos públicos para atualização
            // de description, dueDate e externalReference diretamente
            // Estes campos são atualizados apenas através da integração com Asaas
            // Retornar erro informando que a atualização não é permitida diretamente
            AddError(_messagesService.Payment_Asaas_Error);
            return new FeatureResponse<PaymentResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);

            // Salvar no banco
            await _paymentRepository.UpdateAsync(payment);
            await _context.SaveChangesAsync();

            // Mapear para response
            var response = new PaymentResponse
            {
                Id = payment.Id,
                AsaasPaymentId = payment.AsaasPaymentId,
                BookingId = payment.BookingId,
                Value = payment.Value,
                NetValue = payment.NetValue,
                Method = payment.Method,
                Status = payment.Status,
                InvoiceUrl = payment.InvoiceUrl,
                BankSlipUrl = payment.BankSlipUrl,
                TransactionReceiptUrl = payment.TransactionReceiptUrl,
                PixQrCode = payment.PixQrCode,
                PixEncodedImage = payment.PixEncodedImage,
                DueDate = payment.DueDate,
                ConfirmedDate = payment.ConfirmedDate,
                PaymentDate = payment.PaymentDate,
                CreditDate = payment.CreditDate,
                Description = payment.Description,
                ExternalReference = payment.ExternalReference,
                BillingType = payment.BillingType,
                CreditCardInfo = payment.CreditCardInfo,
                Splits = payment.Splits,
                CreatedAt = payment.CreatedAt!.Value,
                UpdatedAt = payment.UpdatedAt
            };

            _logger.LogInformation("Pagamento {PaymentId} atualizado com sucesso", payment.Id);

            return new FeatureResponse<PaymentResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar pagamento {PaymentId}", request.PaymentId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<PaymentResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}