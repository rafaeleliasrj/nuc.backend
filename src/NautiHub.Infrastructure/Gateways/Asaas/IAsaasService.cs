
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NautiHub.Infrastructure.Gateways.Asaas;

/// <summary>
/// Interface do serviço de integração com gateway de pagamentos Asaas
/// </summary>
public interface IAsaasService
{
    /// <summary>
    /// Criar cliente no Asaas
    /// </summary>
    Task<Result<AsaasCustomer>> CreateCustomerAsync(AsaasCreateCustomerRequest request);

    /// <summary>
    /// Buscar cliente por ID no Asaas
    /// </summary>
    Task<Result<AsaasCustomer>> GetCustomerAsync(string customerId);

    /// <summary>
    /// Criar pagamento no Asaas
    /// </summary>
    Task<Result<AsaasPayment>> CreatePaymentAsync(AsaasCreatePaymentRequest request);

    /// <summary>
    /// Cria pagamento com cartão de crédito no Asaas
    /// </summary>
    Task<Result<AsaasPayment>> CreatePaymentWithCreditCardAsync(AsaasCreatePaymentRequest paymentRequest, AsaasTokenizeCreditCardRequest cardRequest);

    /// <summary>
    /// Buscar pagamento por ID no Asaas
    /// </summary>
    Task<Result<AsaasPayment>> GetPaymentAsync(string paymentId);

    /// <summary>
    /// Gerar QR Code para Pix
    /// </summary>
    Task<Result<AsaasPixQrCode>> GetPixQrCodeAsync(string paymentId);

    /// <summary>
    /// Obter dados do boleto
    /// </summary>
    Task<Result<AsaasBankSlip>> GetBankSlipAsync(string paymentId);

    /// <summary>
    /// Estornar pagamento
    /// </summary>
    Task<Result<AsaasRefund>> RefundPaymentAsync(string paymentId, AsaasRefundRequest request);

    /// <summary>
    /// Tokenizar cartão de crédito
    /// </summary>
    Task<Result<AsaasCreditCardToken>> TokenizeCreditCardAsync(AsaasTokenizeCreditCardRequest request);


}