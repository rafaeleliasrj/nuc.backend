using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.Gateways.Asaas;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;
using System.Linq;
using System.Net;
using MediatR;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace NautiHub.Application.UseCases.Features.PaymentCreate;

/// <summary>
/// Handler para criar pagamento com cartão de crédito
/// </summary>
public class CreatePaymentFeatureHandler : FeatureHandler, IRequestHandler<CreatePaymentFeature, FeatureResponse<CreatePaymentCommandResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IAsaasService _asaasService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<CreatePaymentFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public CreatePaymentFeatureHandler(
        DatabaseContext context,
        IAsaasService asaasService,
        IPaymentRepository paymentRepository,
        IBookingRepository bookingRepository,
        ILogger<CreatePaymentFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _asaasService = asaasService;
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<CreatePaymentCommandResponse>> Handle(CreatePaymentFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar reserva no banco
            var booking = await _context.Set<Booking>().FindAsync(request.Data.BookingId);
            if (booking == null)
            {
                _logger.LogWarning("Reserva {BookingId} não encontrada", request.Data.BookingId);
                AddError(_messagesService.Payment_Booking_Not_Found);
                return new FeatureResponse<CreatePaymentCommandResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // TODO: Validar se a reserva permite pagamentos

            // Criar pagamento no banco
            var payment = new Payment(
                request.Data.BookingId,
                request.Data.Value,
                PaymentMethod.CreditCard,
                request.Data.Description,
                request.Data.ExternalReference
            );

            await _paymentRepository.AddAsync(payment);

            // Primeiro tokenizar o cartão de crédito
            var tokenizeRequest = new AsaasTokenizeCreditCardRequest
            {
                CreditCard = new AsaasCreditCardData
                {
                    HolderName = request.Data.CreditCard.HolderName,
                    Number = request.Data.CreditCard.Number,
                    ExpiryMonth = request.Data.CreditCard.ExpiryMonth,
                    ExpiryYear = request.Data.CreditCard.ExpiryYear,
                    Cvv = request.Data.CreditCard.Cvv
                },
                Customer = request.Data.AsaasCustomerId,
                BillingType = "CREDIT_CARD",
                Value = request.Data.Value,
                Description = request.Data.Description,
                DueDate = request.Data.DueDate,
                RemoteIp = request.Data.CreditCard.RemoteIp
            };

            var tokenResult = await _asaasService.TokenizeCreditCardAsync(tokenizeRequest);
            if (!tokenResult.IsSuccess)
            {
                _logger.LogError("Erro ao tokenizar cartão: {Error}", tokenResult.Error);
                AddError(_messagesService.Payment_General_Error);
                return new FeatureResponse<CreatePaymentCommandResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
            }

            // Criar pagamento no Asaas com o token do cartão
            var paymentRequest = new AsaasCreatePaymentRequest
            {
                Customer = request.Data.AsaasCustomerId,
                BillingType = "CREDIT_CARD",
                Description = request.Data.Description,
                Value = request.Data.Value,
                DueDate = request.Data.DueDate,
                ExternalReference = payment.Id.ToString()
            };

            var cardResult = await _asaasService.CreatePaymentWithCreditCardAsync(paymentRequest, tokenizeRequest);

            if (!cardResult.IsSuccess)
            {
                _logger.LogError("Erro ao criar pagamento com cartão: {Error}", cardResult.Error);
                AddError(_messagesService.Payment_General_Error);
                return new FeatureResponse<CreatePaymentCommandResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
            }

            // Atualizar ID do Asaas
            payment.UpdateAsaasInfo(cardResult.Data.Id, payment.Value);
            await _paymentRepository.UpdateAsync(payment);

            // Processar splits se existirem
            if (request.Data.Splits?.Any() == true)
            {
                // TODO: Implementar lógica de splits
            }

            // Mapear status
            var status = MapAsaasStatusToPaymentStatus(cardResult.Data.Status);

            // Atualizar status do pagamento
            payment.UpdateStatus(status);
            if (status == PaymentStatus.Paid)
            {
                payment.Confirm();
            }
            await _paymentRepository.UpdateAsync(payment);

            // TODO: Enviar notificações

            // Montar response
            var response = new CreatePaymentCommandResponse
            {
                PaymentId = payment.Id,
                Status = payment.Status.ToString(),
                AsaasPaymentId = payment.AsaasPaymentId,
                InvoiceUrl = payment.InvoiceUrl,
                BankSlipUrl = payment.BankSlipUrl,
                PixQrCode = payment.PixQrCode,
                PixPayload = payment.PixQrCode, // Simplificado - pode ser ajustado se necessário
                DueDate = payment.DueDate,
                NetValue = payment.NetValue
            };

            _logger.LogInformation("Pagamento {PaymentId} criado com sucesso para reserva {BookingId}", payment.Id, request.Data.BookingId);

            return new FeatureResponse<CreatePaymentCommandResponse>(ValidationResult, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar comando de criar pagamento");
            AddError(_messagesService.Payment_General_Error);
            return new FeatureResponse<CreatePaymentCommandResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// Mapear status do Asaas para status do domínio
    /// </summary>
    private static PaymentStatus MapAsaasStatusToPaymentStatus(string asaasStatus)
    {
        return asaasStatus switch
        {
            "PENDING" or "AWAITING_PAYMENT" => PaymentStatus.Pending,
            "CONFIRMED" or "RECEIVED" => PaymentStatus.Paid,
            "OVERDUE" or "REFUSED" => PaymentStatus.Failed,
            "REFUNDED" => PaymentStatus.Refunded,
            "PARTIALLY_REFUNDED" => PaymentStatus.PartiallyRefunded,
            _ => PaymentStatus.Pending
        };
    }
}