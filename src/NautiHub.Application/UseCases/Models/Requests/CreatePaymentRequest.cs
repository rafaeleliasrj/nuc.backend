using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para criação de pagamento
/// </summary>
public class CreatePaymentRequest
{
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
    public PaymentMethod Method { get; set; }

    /// <summary>
    /// Descrição do pagamento
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Data de vencimento
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Referência externa
    /// </summary>
    public string ExternalReference { get; set; }

    /// <summary>
    /// Splits de pagamento
    /// </summary>
    public List<PaymentSplitRequest> Splits { get; set; }

    /// <summary>
    /// Dados do cartão de crédito
    /// </summary>
    public CreditCardRequest CreditCard { get; set; }
}