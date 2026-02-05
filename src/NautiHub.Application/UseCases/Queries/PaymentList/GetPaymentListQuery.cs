using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Queries.PaymentList;

/// <summary>
/// Query para listar pagamentos
/// </summary>
public class GetPaymentListQuery : Query<QueryResponse<PaymentListResponse>>
{
    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Quantidade de itens por página
    /// </summary>
    public int PageSize { get; set; } = 10;

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