using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NautiHub.Core.Resources;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;

namespace NautiHub.Infrastructure.Gateways.Asaas;

/// <summary>
/// Serviço de integração com gateway de pagamentos Asaas
/// </summary>
public class AsaasService : IAsaasService
{
    private readonly IAsaasPaymentsApi _paymentsApi;
    private readonly IAsaasCustomersApi _customersApi;
    private readonly HttpClient _httpClient;
    private readonly AsaasSettings _settings;
    private readonly ILogger<AsaasService> _logger;
    private readonly MessagesService _messagesService;

    public AsaasService(
        IAsaasPaymentsApi paymentsApi,
        IAsaasCustomersApi customersApi,
        HttpClient httpClient,
        IOptions<AsaasSettings> settings,
        ILogger<AsaasService> logger,
        MessagesService messagesService)
    {
        _paymentsApi = paymentsApi;
        _customersApi = customersApi;
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
        _messagesService = messagesService;
    }

    /// <summary>
    /// Criar pagamento no Asaas
    /// </summary>
    public async Task<Result<AsaasPayment>> CreatePaymentAsync(AsaasCreatePaymentRequest request)
    {
        try
        {
            var response = await _paymentsApi.CreatePaymentAsync(request);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasPayment>.Success(response.Content);
            }

            return Result<AsaasPayment>.Failure(_messagesService.Payment_Create_Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pagamento no Asaas");
            return Result<AsaasPayment>.Failure(_messagesService.Payment_Create_Internal_Error);
        }
    }

    /// <summary>
    /// Criar pagamento com cartão de crédito (inclui tokenização)
    /// </summary>
    public async Task<Result<AsaasPayment>> CreatePaymentWithCreditCardAsync(
        AsaasCreatePaymentRequest paymentRequest,
        AsaasTokenizeCreditCardRequest cardRequest)
    {
        try
        {
            // Primeiro tokenizar o cartão
            var tokenResult = await TokenizeCreditCardAsync(cardRequest);
            if (!tokenResult.IsSuccess)
            {
                return Result<AsaasPayment>.Failure($"{_messagesService.Payment_Token_Error}: {tokenResult.Error}");
            }

            // Criar pagamento com o token
            var response = await _paymentsApi.CreatePaymentAsync(paymentRequest);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasPayment>.Success(response.Content);
            }

            return Result<AsaasPayment>.Failure(_messagesService.Payment_Card_Create_Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pagamento com cartão no Asaas");
            return Result<AsaasPayment>.Failure(_messagesService.Payment_Card_Internal_Error);
        }
    }

    /// <summary>
    /// Buscar pagamento por ID
    /// </summary>
    public async Task<Result<AsaasPayment>> GetPaymentAsync(string paymentId)
    {
        try
        {
            var response = await _paymentsApi.GetPaymentAsync(paymentId);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasPayment>.Success(response.Content);
            }

            return Result<AsaasPayment>.Failure(_messagesService.Payment_Not_Found);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pagamento {PaymentId} no Asaas", paymentId);
            return Result<AsaasPayment>.Failure(_messagesService.Payment_Get_Internal_Error);
        }
    }

    /// <summary>
    /// Gerar QR Code para Pix
    /// </summary>
    public async Task<Result<AsaasPixQrCode>> GetPixQrCodeAsync(string paymentId)
    {
        try
        {
            var response = await _paymentsApi.GetPixQrCodeAsync(paymentId);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasPixQrCode>.Success(response.Content);
            }

            return Result<AsaasPixQrCode>.Failure(_messagesService.Payment_QRCode_Not_Allowed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar QR Code {PaymentId} no Asaas", paymentId);
            return Result<AsaasPixQrCode>.Failure(_messagesService.Payment_QRCode_Internal_Error);
        }
    }

    /// <summary>
    /// Obter dados do boleto
    /// </summary>
    public async Task<Result<AsaasBankSlip>> GetBankSlipAsync(string paymentId)
    {
        try
        {
            var response = await _paymentsApi.GetBankSlipAsync(paymentId);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasBankSlip>.Success(response.Content);
            }

            return Result<AsaasBankSlip>.Failure(_messagesService.Payment_Boleto_Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar boleto {PaymentId} no Asaas", paymentId);
            return Result<AsaasBankSlip>.Failure(_messagesService.Payment_Boleto_Internal_Error);
        }
    }

    /// <summary>
    /// Estornar pagamento
    /// </summary>
    public async Task<Result<AsaasRefund>> RefundPaymentAsync(string paymentId, AsaasRefundRequest request)
    {
        try
        {
            var response = await _paymentsApi.RefundPaymentAsync(paymentId, request);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasRefund>.Success(response.Content);
            }

            return Result<AsaasRefund>.Failure(_messagesService.Payment_Refund_Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao estornar pagamento {PaymentId} no Asaas", paymentId);
            return Result<AsaasRefund>.Failure(_messagesService.Payment_Refund_Internal_Error);
        }
    }

    /// <summary>
    /// Tokenizar cartão de crédito
    /// </summary>
    public async Task<Result<AsaasCreditCardToken>> TokenizeCreditCardAsync(AsaasTokenizeCreditCardRequest request)
    {
        try
        {
            var response = await _paymentsApi.TokenizeCreditCardAsync(request);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasCreditCardToken>.Success(response.Content);
            }

            return Result<AsaasCreditCardToken>.Failure(_messagesService.Payment_Token_Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao tokenizar cartão no Asaas");
            return Result<AsaasCreditCardToken>.Failure(_messagesService.Payment_Token_Error);
        }
    }

    /// <summary>
    /// Criar cliente no Asaas
    /// </summary>
    public async Task<Result<AsaasCustomer>> CreateCustomerAsync(AsaasCreateCustomerRequest request)
    {
        try
        {
            var response = await _customersApi.CreateCustomerAsync(request);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasCustomer>.Success(response.Content);
            }

            return Result<AsaasCustomer>.Failure(_messagesService.Error_Registering_User);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cliente no Asaas");
            return Result<AsaasCustomer>.Failure(_messagesService.Error_Internal_Server);
        }
    }

    /// <summary>
    /// Buscar cliente por ID
    /// </summary>
    public async Task<Result<AsaasCustomer>> GetCustomerAsync(string customerId)
    {
        try
        {
            var response = await _customersApi.GetCustomerAsync(customerId);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return Result<AsaasCustomer>.Success(response.Content);
            }

            return Result<AsaasCustomer>.Failure(_messagesService.Auth_User_Not_Found);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar cliente {CustomerId} no Asaas", customerId);
            return Result<AsaasCustomer>.Failure(_messagesService.Error_Internal_Server);
        }
    }
}