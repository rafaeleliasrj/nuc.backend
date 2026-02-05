using NautiHub.Infrastructure.Gateways.Asaas.DTOs;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response da query de buscar pagamento por ID
/// </summary>
public class PaymentByIdResponse
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID da reserva
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// ID do cliente no Asaas
    /// </summary>
    public string AsaasCustomerId { get; set; }

    /// <summary>
    /// Valor do pagamento
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Método de pagamento
    /// </summary>
    public string Method { get; set; }

    /// <summary>
    /// Status do pagamento
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data de vencimento
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Data de pagamento confirmado
    /// </summary>
    public DateTime? PaidAt { get; set; }

    /// <summary>
    /// Descrição
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Referência externa
    /// </summary>
    public string ExternalReference { get; set; }

    /// <summary>
    /// URL do boleto (se aplicável)
    /// </summary>
    public string BankSlipUrl { get; set; }

    /// <summary>
    /// QR Code do Pix (se aplicável)
    /// </summary>
    public string PixQrCode { get; set; }

    /// <summary>
    /// Informações do cartão (mascaradas)
    /// </summary>
    public string CreditCardInfo { get; set; }
}