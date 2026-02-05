namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response de avaliação
/// </summary>
public class ReviewResponse
{
    /// <summary>
    /// Identificador da avaliação
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador da reserva avaliada
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Identificador da embarcação avaliada
    /// </summary>
    public Guid BoatId { get; set; }

    /// <summary>
    /// Identificador do cliente que fez a avaliação
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Classificação da avaliação (1 a 5 estrelas)
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Comentário da avaliação
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Data de criação da avaliação
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}