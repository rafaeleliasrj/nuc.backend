using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;

namespace NautiHub.Infrastructure.Gateways.Asaas;

/// <summary>
/// Interface para comunicação com API de pagamentos Asaas
/// </summary>
[Headers("User-Agent: NautiHub/1.0")]
public interface IAsaasPaymentsApi
{
    [Post("/payments")]
    Task<Refit.ApiResponse<AsaasPayment>> CreatePaymentAsync([Body] AsaasCreatePaymentRequest request);

    [Post("/payments")]
    Task<Refit.ApiResponse<AsaasPayment>> CreatePaymentWithCreditCardAsync([Body] AsaasCreatePaymentWithCreditCardRequest request);

    [Get("/payments/{id}")]
    Task<Refit.ApiResponse<AsaasPayment>> GetPaymentAsync(string id);

    [Get("/payments/{id}/pixQrCode")]
    Task<Refit.ApiResponse<AsaasPixQrCode>> GetPixQrCodeAsync(string id);

    [Get("/payments/{id}/bankSlip")]
    Task<Refit.ApiResponse<AsaasBankSlip>> GetBankSlipAsync(string id);

    [Post("/payments/{id}/refund")]
    Task<Refit.ApiResponse<AsaasRefund>> RefundPaymentAsync(string id, [Body] AsaasRefundRequest request);

    [Post("/creditCard/token")]
    Task<Refit.ApiResponse<AsaasCreditCardToken>> TokenizeCreditCardAsync([Body] AsaasTokenizeCreditCardRequest request);

    [Get("/payments")]
    Task<Refit.ApiResponse<PagedResponse<AsaasPayment>>> ListPaymentsAsync(
        [Query] int? limit = null,
        [Query] int? offset = null,
        [Query] string customer = null,
        [Query] string billingType = null,
        [Query] string status = null,
        [Query] DateTime? dueDate = null,
        [Query] string externalReference = null);
}