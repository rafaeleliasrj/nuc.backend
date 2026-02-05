using Microsoft.Extensions.Logging;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Infrastructure.Gateways.Asaas;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Features.GeneratePixQrCode;

/// <summary>
/// Handler para gerar QR Code Pix
/// </summary>
public class GeneratePixQrCodeFeatureHandler(
    DatabaseContext context,
    IPaymentRepository paymentRepository,
    IAsaasService asaasService,
    ILogger<GeneratePixQrCodeFeatureHandler> logger,
    MessagesService messagesService) : FeatureHandler(context)
{
    private readonly DatabaseContext _context = context;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IAsaasService _asaasService = asaasService;
    private readonly ILogger<GeneratePixQrCodeFeatureHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<FeatureResponse<GeneratePixQrCodeResponse>> Handle(GeneratePixQrCodeFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar pagamento no banco
            var payment = await _context.Set<Payment>().FindAsync(request.PaymentId);
            if (payment == null)
            {
                AddError(_messagesService.Payment_Not_Found);
                return new FeatureResponse<GeneratePixQrCodeResponse>(ValidationResult);
            }

            // Validar se o pagamento permite QR Code Pix
            if (!CanGeneratePixQrCode(payment))
            {
                _logger.LogWarning("Pagamento {PaymentId} não permite geração de QR Code Pix. Status: {Status}, Método: {Method}", 
                    payment.Id, payment.Status, payment.Method);
                
                AddError(_messagesService.Payment_QRCode_Not_Allowed);
                return new FeatureResponse<GeneratePixQrCodeResponse>(ValidationResult);
            }

            // Se não tiver ID do Asaas, retornar erro
            if (string.IsNullOrEmpty(payment.AsaasPaymentId))
            {
                _logger.LogWarning("Pagamento {PaymentId} não possui ID do Asaas", payment.Id);
                
                AddError(_messagesService.Payment_No_Asaas_Id);
                return new FeatureResponse<GeneratePixQrCodeResponse>(ValidationResult);
            }

            // Buscar QR Code no Asaas
            var qrCodeResult = await _asaasService.GetPixQrCodeAsync(payment.AsaasPaymentId);
            if (!qrCodeResult.IsSuccess)
            {
                _logger.LogError("Erro ao buscar QR Code no Asaas para pagamento {PaymentId}: {Error}", 
                    payment.Id, qrCodeResult.Error);
                
                AddError(_messagesService.Payment_Asaas_Error);
                return new FeatureResponse<GeneratePixQrCodeResponse>(ValidationResult);
            }

            var qrCode = qrCodeResult.Data;

            // Montar response
            var response = new GeneratePixQrCodeResponse
            {
                PaymentId = payment.Id,
                QrCodeImage = qrCode.QrCode.Image,
                Payload = qrCode.QrCode.Content,
                Value = payment.Value,
                Status = payment.Status.ToString()
            };

            _logger.LogInformation("QR Code gerado com sucesso para pagamento {PaymentId}", payment.Id);

            return new FeatureResponse<GeneratePixQrCodeResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar QR Code para pagamento {PaymentId}", request.PaymentId);
            
            AddError(_messagesService.Payment_General_Error);
            return new FeatureResponse<GeneratePixQrCodeResponse>(ValidationResult);
        }
    }

    /// <summary>
    /// Verifica se é possível gerar QR Code Pix para o pagamento
    /// </summary>
    private static bool CanGeneratePixQrCode(Payment payment)
    {
        // Apenas pagamentos do tipo PIX podem gerar QR Code
        if (payment.Method != PaymentMethod.Pix)
            return false;

        // Pagamento deve estar pendente ou pago
        if (payment.Status != Domain.Enums.PaymentStatus.Pending && payment.Status != Domain.Enums.PaymentStatus.Paid)
            return false;

        // Deve ter valor positivo
        if (payment.Value <= 0)
            return false;

        return true;
    }
}