using NautiHub.Core.Messages.Models;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Item da lista de pagamentos
/// </summary>
public class PaymentListItem
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
    /// Descrição
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// Response da query de listagem de pagamentos
/// </summary>
public class ListPaymentsResponse
{
    /// <summary>
    /// Lista de pagamentos
    /// </summary>
    public List<PaymentListItem> Payments { get; set; } = new();

    /// <summary>
    /// Paginação
    /// </summary>
    public PaginationInfo Pagination { get; set; }
}