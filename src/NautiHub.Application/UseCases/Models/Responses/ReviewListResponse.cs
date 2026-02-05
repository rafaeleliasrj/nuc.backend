namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response de lista de avaliações
/// </summary>
public class ReviewListResponse
{
    /// <summary>
    /// Lista de avaliações
    /// </summary>
    public IList<ReviewResponse> Reviews { get; set; } = new List<ReviewResponse>();

    /// <summary>
    /// Total de avaliações encontradas
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
    /// Média de avaliações
    /// </summary>
    public double AverageRating { get; set; }

    /// <summary>
    /// Filtros aplicados
    /// </summary>
    public ReviewFilters Filters { get; set; } = new();
}

/// <summary>
/// Filtros utilizados na consulta de avaliações
/// </summary>
public class ReviewFilters
{
    /// <summary>
    /// ID da reserva filtrada
    /// </summary>
    public Guid? BookingId { get; set; }

    /// <summary>
    /// ID da embarcação filtrada
    /// </summary>
    public Guid? BoatId { get; set; }

    /// <summary>
    /// ID do cliente filtrado
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Classificação mínima
    /// </summary>
    public int? MinRating { get; set; }

    /// <summary>
    /// Classificação máxima
    /// </summary>
    public int? MaxRating { get; set; }

    /// <summary>
    /// Data inicial do período
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data final do período
    /// </summary>
    public DateTime? EndDate { get; set; }
}