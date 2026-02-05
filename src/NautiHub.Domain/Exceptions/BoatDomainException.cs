using NautiHub.Core.DomainObjects;

namespace NautiHub.Domain.Exceptions;

/// <summary>
/// Exceções de domínio específicas para a entidade Boat
/// </summary>
public class BoatDomainException : DomainException
{
    public string MessageKey { get; }

    public BoatDomainException(string messageKey, string defaultMessage) 
        : base(defaultMessage)
    {
        MessageKey = messageKey;
    }

    public BoatDomainException(string messageKey, string defaultMessage, Exception innerException) 
        : base(defaultMessage, innerException)
    {
        MessageKey = messageKey;
    }

    // Métodos fáceis para criar exceções específicas
    public static BoatDomainException NameRequired() => 
        new("Boat_Name_Required", "Nome da embarcação é obrigatório.");

    public static BoatDomainException NameTooLong() => 
        new("Boat_Name_Too_Long", "Nome da embarcação não pode exceder 200 caracteres.");

    public static BoatDomainException DescriptionRequired() => 
        new("Boat_Description_Required", "Descrição da embarcação é obrigatória.");

    public static BoatDomainException DescriptionTooLong() => 
        new("Boat_Description_Too_Long", "Descrição da embarcação não pode exceder 2000 caracteres.");

    public static BoatDomainException DocumentRequired() => 
        new("Boat_Document_Required", "Documento da embarcação é obrigatório.");

    public static BoatDomainException DocumentTooLong() => 
        new("Boat_Document_Too_Long", "Documento da embarcação não pode exceder 50 caracteres.");

    public static BoatDomainException CapacityInvalid() => 
        new("Boat_Capacity_Invalid", "Capacidade deve ser maior que zero.");

    public static BoatDomainException CapacityTooHigh() => 
        new("Boat_Capacity_Too_High", "Capacidade não pode exceder 1000 pessoas.");

    public static BoatDomainException PriceInvalid() => 
        new("Boat_Price_Invalid", "Preço por pessoa deve ser maior que zero.");

    public static BoatDomainException PriceTooHigh() => 
        new("Boat_Price_Too_High", "Preço por pessoa não pode exceder R$ 100.000,00.");

    public static BoatDomainException CityRequired() => 
        new("Boat_City_Required", "Cidade da localização é obrigatória.");

    public static BoatDomainException CityTooLong() => 
        new("Boat_City_Too_Long", "Cidade da localização não pode exceder 100 caracteres.");

    public static BoatDomainException StateRequired() => 
        new("Boat_State_Required", "Estado da localização é obrigatório.");

    public static BoatDomainException StateInvalidFormat() => 
        new("Boat_State_Invalid_Format", "Estado deve ter exatamente 2 caracteres.");

    public static BoatDomainException OnlyApprovedCanBeActive() => 
        new("Boat_Only_Approved_Can_Be_Active", "Apenas embarcações aprovadas podem ser ativadas.");

    public static BoatDomainException AlreadyApproved() => 
        new("Boat_Already_Approved", "Embarcação já está aprovada.");

    public static BoatDomainException CannotRejectApproved() => 
        new("Boat_Cannot_Reject_Approved", "Não é possível rejeitar uma embarcação já aprovada.");

    public static BoatDomainException OnlyApprovedCanBeSuspended() => 
        new("Boat_Only_Approved_Can_Be_Suspended", "Apenas embarcações aprovadas podem ser suspensas.");

    public static BoatDomainException OnlySuspendedCanBeReactived() => 
        new("Boat_Only_Suspended_Can_Be_Reactived", "Apenas embarcações suspensas podem ser reativadas.");
}