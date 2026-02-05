using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;
using NautiHub.Infrastructure.DataContext;
using NautiHub.Infrastructure.Gateways.Asaas;
using System.Net;
using NautiHub.Domain.Repositories;

namespace NautiHub.Application.UseCases.Features.GenerateBoleto;

/// <summary>
/// Handler para gerar boleto de pagamento
/// </summary>
public class GenerateBoletoFeatureHandler(
    DatabaseContext context,
    IPaymentRepository paymentRepository,
    IAsaasService asaasService,
    ILogger<GenerateBoletoFeatureHandler> logger,
    MessagesService messagesService
) : FeatureHandler(context), IRequestHandler<GenerateBoletoFeature, FeatureResponse<GenerateBoletoResponse>>
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IAsaasService _asaasService = asaasService;
    private readonly ILogger<GenerateBoletoFeatureHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<FeatureResponse<GenerateBoletoResponse>> Handle(GenerateBoletoFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar pagamento no banco
            var payment = await _paymentRepository.GetByIdAsync(request.PaymentId);
            if (payment == null)
            {
                _logger.LogWarning("Pagamento {PaymentId} não encontrado", request.PaymentId);
                AddError(_messagesService.Payment_Not_Found);
                return new FeatureResponse<GenerateBoletoResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Validar se o pagamento permite geração de boleto
            if (!CanGenerateBoleto(payment))
            {
                _logger.LogWarning("Pagamento {PaymentId} não permite geração de boleto. Status: {Status}, Método: {Method}", 
                    request.PaymentId, payment.Status, payment.Method);
                AddError(_messagesService.Payment_Invalid_Method);
                return new FeatureResponse<GenerateBoletoResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            // Se não tiver ID do Asaas, retornar erro
            if (string.IsNullOrEmpty(payment.AsaasPaymentId))
            {
                _logger.LogWarning("Pagamento {PaymentId} não possui ID do Asaas", request.PaymentId);
                var validationResult = new ValidationResult();
                AddError(_messagesService.Payment_No_Asaas_Id);
                return new FeatureResponse<GenerateBoletoResponse>(validationResult, statusCode: HttpStatusCode.BadRequest);
            }

            // Buscar dados do boleto no Asaas
            var bankSlipResult = await _asaasService.GetBankSlipAsync(payment.AsaasPaymentId);
            if (!bankSlipResult.IsSuccess)
            {
                _logger.LogError("Erro ao buscar boleto no Asaas para pagamento {PaymentId}: {Error}", 
                    request.PaymentId, bankSlipResult.Error);
                AddError(_messagesService.Payment_Asaas_Error);
                return new FeatureResponse<GenerateBoletoResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            var bankSlip = bankSlipResult.Data;

            // Montar response
            var response = new GenerateBoletoResponse
            {
                PaymentId = payment.Id,
                BankSlipUrl = bankSlip.BankSlipUrl,
                DigitableLine = bankSlip.DigitableLine,
                BarcodeNumber = bankSlip.BarcodeNumber,
                NossoNumero = bankSlip.NossoNumero,
                DueDate = payment.DueDate,
                Value = payment.Value,
                Status = payment.Status.ToString()
            };

            _logger.LogInformation("Boleto gerado com sucesso para pagamento {PaymentId}", request.PaymentId);

            return new FeatureResponse<GenerateBoletoResponse>(ValidationResult, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar boleto para pagamento {PaymentId}", request.PaymentId);
            AddError(_messagesService.Payment_General_Error);
            return new FeatureResponse<GenerateBoletoResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }

    private static bool CanGenerateBoleto(Payment payment)
    {
        // Só pode gerar boleto para pagamentos BOLETO ou UNDEFINED em status PENDING
        return (payment.Method == PaymentMethod.Boleto || payment.Method == PaymentMethod.Undefined) &&
               payment.Status == PaymentStatus.Pending &&
               !string.IsNullOrEmpty(payment.AsaasPaymentId);
    }
}