using NautiHub.Core.DomainObjects;

namespace NautiHub.Domain.Exceptions;

/// <summary>
/// Exceções de domínio específicas para a entidade User
/// </summary>
public class UserDomainException : DomainException
{
    public string MessageKey { get; }

    public UserDomainException(string messageKey, string defaultMessage) 
        : base(defaultMessage)
    {
        MessageKey = messageKey;
    }

    public UserDomainException(string messageKey, string defaultMessage, Exception innerException) 
        : base(defaultMessage, innerException)
    {
        MessageKey = messageKey;
    }

    // Métodos fáceis para criar exceções específicas
    public static UserDomainException FullNameRequired() => 
        new("Auth_User_Not_Identified", "Full name is required");

    public static UserDomainException InvalidBirthDate() => 
        new("Validation_Invalid_Date", "Invalid date of birth");
}