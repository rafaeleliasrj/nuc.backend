using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Queries.PaymentById;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Communication;
using NautiHub.Core.Messages.Queries;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Infrastructure.Gateways.Asaas;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Queries.PaymentById;

/// <summary>
/// Handler para buscar pagamento por ID
/// </summary>
public class GetPaymentByIdQueryHandler(
    DatabaseContext context,
    IPaymentRepository paymentRepository,
    IAsaasService asaasService,
    ILogger<GetPaymentByIdQueryHandler> logger,
    MessagesService messagesService) : QueryHandler
{
    private readonly DatabaseContext _context = context;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IAsaasService _asaasService = asaasService;
    private readonly ILogger<GetPaymentByIdQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<PaymentByIdResponse>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar pagamento no banco
            var payment = await _context.Set<Payment>().FindAsync(request.Id);
            if (payment == null)
            {
                var validationResult = new ValidationResult();
                validationResult.Errors.Add(new ValidationFailure("PaymentId", _messagesService.Payment_Not_Found));
                return new QueryResponse<PaymentByIdResponse>(validationResult);
            }

            // Se tiver ID do Asaas, buscar informações atualizadas
            AsaasPayment asaasPayment = null;
            string pixQrCode = null;
            
            if (!string.IsNullOrEmpty(payment.AsaasPaymentId))
            {
                var asaasResult = await _asaasService.GetPaymentAsync(payment.AsaasPaymentId);
                if (asaasResult.IsSuccess)
                {
                    asaasPayment = asaasResult.Data;
                    
                    // Se for pagamento PIX, buscar QR Code separadamente
                    if (payment.Method == PaymentMethod.Pix)
                    {
                        var qrCodeResult = await _asaasService.GetPixQrCodeAsync(payment.AsaasPaymentId);
                        if (qrCodeResult.IsSuccess && qrCodeResult.Data != null)
                        {
                            pixQrCode = qrCodeResult.Data.QrCode.Image ?? payment.PixEncodedImage;
                        }
                    }
                }
            }

            // Montar response
            var response = new PaymentByIdResponse
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                AsaasCustomerId = "", // TODO: Adicionar esta propriedade na entidade Payment
                Value = payment.Value,
                Method = payment.Method.ToString(),
                Status = payment.Status.ToString(),
                CreatedAt = payment.CreatedAt ?? DateTime.Now,
                DueDate = payment.DueDate,
                PaidAt = payment.ConfirmedDate ?? DateTime.MinValue,
                Description = payment.Description,
                ExternalReference = payment.ExternalReference,
                BankSlipUrl = asaasPayment?.BankSlipUrl ?? payment.BankSlipUrl,
                PixQrCode = pixQrCode ?? payment.PixEncodedImage,
                CreditCardInfo = payment.CreditCardInfo?.GetMaskedNumber() ?? ""
            };

            return new QueryResponse<PaymentByIdResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pagamento {PaymentId}", request.Id);
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("General", _messagesService.Payment_General_Error));
            return new QueryResponse<PaymentByIdResponse>(validationResult);
        }
    }
}