using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Queries.ScheduledTourList;

/// <summary>
/// Query para listar passeios agendados
/// </summary>
public class GetScheduledTourListQuery : Query<QueryResponse<ScheduledTourListResponse>>
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
    /// ID do barco filtrado
    /// </summary>
    public Guid? BoatId { get; set; }

    /// <summary>
    /// Status do passeio filtrado
    /// </summary>
    public ScheduledTourStatus? Status { get; set; }

    /// <summary>
    /// Data inicial do período
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// Data final do período
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Indica se busca apenas passeios ativos
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Número mínimo de assentos disponíveis
    /// </summary>
    public int? MinAvailableSeats { get; set; }
}