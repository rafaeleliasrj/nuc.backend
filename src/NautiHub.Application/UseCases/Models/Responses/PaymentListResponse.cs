using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response de lista de pagamentos
/// </summary>
public class PaymentListResponse
{
    /// <summary>
    /// Lista de pagamentos
    /// </summary>
    public IList<PaymentResponse> Payments { get; set; } = new List<PaymentResponse>();

    /// <summary>
    /// Total de pagamentos encontrados
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Quantidade de itens por página
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de páginas
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Filtros aplicados
    /// </summary>
    public PaymentFilters Filters { get; set; } = new();
}

/// <summary>
/// Filtros utilizados na consulta de pagamentos
/// </summary>
public class PaymentFilters
{
    /// <summary>
    /// ID da reserva filtrada
    /// </summary>
    public Guid? BookingId { get; set; }

    /// <summary>
    /// Status do pagamento filtrado
    /// </summary>
    public PaymentStatus? Status { get; set; }

    /// <summary>
    /// Método de pagamento filtrado
    /// </summary>
    public PaymentMethod? Method { get; set; }

    /// <summary>
    /// Data inicial do período
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data final do período
    /// </summary>
    public DateTime? EndDate { get; set; }
}