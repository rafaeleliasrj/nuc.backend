using NautiHub.Core.DomainObjects;

namespace NautiHub.Domain.Exceptions;

/// <summary>
/// Exceções de domínio específicas para a entidade Review
/// </summary>
public class ReviewDomainException : DomainException
{
    public string MessageKey { get; }

    public ReviewDomainException(string messageKey, string defaultMessage) 
        : base(defaultMessage)
    {
        MessageKey = messageKey;
    }

    public ReviewDomainException(string messageKey, string defaultMessage, Exception innerException) 
        : base(defaultMessage, innerException)
    {
        MessageKey = messageKey;
    }

    // Métodos fáceis para criar exceções específicas
    public static ReviewDomainException AlreadyExists() => 
        new("Review_Already_Exists", "Já existe uma avaliação para esta reserva");

    public static ReviewDomainException NotFound() => 
        new("Review_Not_Found", "Avaliação {0} não encontrada");

    public static ReviewDomainException RatingInvalid() => 
        new("Validation_Rating_Between_1_5", "Rating must be between 1 and 5");

    public static ReviewDomainException CommentTooLong() => 
        new("Validation_Comment_Too_Long", "Comment cannot exceed 1000 characters");
}