using NautiHub.Core.DomainObjects;
using NautiHub.Core.Utils;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Entities;

/// <summary>
/// Entidade de domínio que representa uma avaliação de reserva
/// </summary>
public class Review : Entity, IAggregateRoot
{
    /// <summary>
    /// Identificador da reserva avaliada
    /// </summary>
    public Guid BookingId { get; private set; }
    public Booking Booking { get; private set; }

    /// <summary>
    /// Identificador da embarcação avaliada
    /// </summary>
    public Guid BoatId { get; private set; }
    public Boat Boat { get; private set; }

    /// <summary>
    /// Identificador do cliente que fez a avaliação
    /// </summary>
    public Guid CustomerId { get; private set; }
    public User Customer { get; private set; }

    /// <summary>
    /// Classificação da avaliação (1 a 5 estrelas)
    /// </summary>
    public int Rating { get; private set; }

    /// <summary>
    /// Comentário da avaliação
    /// </summary>
    public string? Comment { get; private set; }

    // Construtor privado para EF Core
    private Review() { }

    public Review(
        Guid bookingId,
        Guid boatId,
        Guid customerId,
        int rating,
        string? comment = null)
    {
        Id = SequentialGuidGenerator.NewSequentialGuid();
        BookingId = bookingId;
        BoatId = boatId;
        CustomerId = customerId;
        Rating = rating;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o comentário da avaliação
    /// </summary>
    public void UpdateComment(string? comment)
    {
        Comment = comment;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza a classificação da avaliação
    /// </summary>
    public void UpdateRating(int rating)
    {
        if (rating < 1 || rating > 5)
            throw ReviewDomainException.RatingInvalid();
        
        Rating = rating;
        UpdatedAt = DateTime.UtcNow;
    }
}