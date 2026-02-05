using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response de lista de passeios agendados
/// </summary>
public class ScheduledTourListResponse
{
    /// <summary>
    /// Lista de passeios
    /// </summary>
    public IList<ScheduledTourResponse> ScheduledTours { get; set; } = new List<ScheduledTourResponse>();

    /// <summary>
    /// Total de passeios encontrados
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
    public ScheduledTourFilters Filters { get; set; } = new();
}

/// <summary>
/// Filtros utilizados na consulta de passeios agendados
/// </summary>
public class ScheduledTourFilters
{
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