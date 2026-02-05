using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ListPayments;

/// <summary>
/// Query para listar pagamentos com filtros
/// </summary>
public class ListPaymentsQuery : Query<QueryResponse<ListPaymentsResponse>>
{
    /// <summary>
    /// ID da reserva (opcional)
    /// </summary>
    public Guid? BookingId { get; set; }

    /// <summary>
    /// Status do pagamento (opcional)
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Método de pagamento (opcional)
    /// </summary>
    public string Method { get; set; }

    /// <summary>
    /// Número da página
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Tamanho da página
    /// </summary>
    public int PageSize { get; set; } = 10;
}