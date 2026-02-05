using NautiHub.Core.DomainObjects;

namespace NautiHub.Domain.Exceptions;

/// <summary>
/// Exceções de domínio específicas para a entidade Advertisement
/// </summary>
public class AdvertisementDomainException : DomainException
{
    public string MessageKey { get; }

    public AdvertisementDomainException(string messageKey, string defaultMessage) 
        : base(defaultMessage)
    {
        MessageKey = messageKey;
    }

    public AdvertisementDomainException(string messageKey, string defaultMessage, Exception innerException) 
        : base(defaultMessage, innerException)
    {
        MessageKey = messageKey;
    }

    // Métodos fáceis para criar exceções específicas
    public static AdvertisementDomainException AlreadyApproved() => 
        new("Advertisement_Already_Approved", "Anúncio já está aprovado.");

    public static AdvertisementDomainException CannotApproveCanceled() => 
        new("Advertisement_Cannot_Approve_Canceled", "Não é possível aprovar um anúncio cancelado.");

    public static AdvertisementDomainException CannotRejectApproved() => 
        new("Advertisement_Cannot_Reject_Approved", "Não é possível rejeitar um anúncio já aprovado.");

    public static AdvertisementDomainException CannotRejectCanceled() => 
        new("Advertisement_Cannot_Reject_Canceled", "Não é possível rejeitar um anúncio cancelado.");

    public static AdvertisementDomainException OnlyApprovedCanSuspend() => 
        new("Advertisement_Only_Approved_Can_Suspend", "Apenas anúncios aprovados podem ser suspensos.");

    public static AdvertisementDomainException OnlySuspendedCanReactive() => 
        new("Advertisement_Only_Suspended_Can_Reactive", "Apenas anúncios suspensos podem ser reativados.");

    public static AdvertisementDomainException OnlyApprovedCanPause() => 
        new("Advertisement_Only_Approved_Can_Pause", "Apenas anúncios aprovados podem ser pausados.");

    public static AdvertisementDomainException OnlyPausedCanUnpause() => 
        new("Advertisement_Only_Paused_Can_Unpause", "Apenas anúncios pausados podem ser despausados.");

    public static AdvertisementDomainException AlreadyCanceled() => 
        new("Advertisement_Already_Canceled", "Anúncio já está cancelado.");

    public static AdvertisementDomainException CannotCancelCompleted() => 
        new("Advertisement_Cannot_Cancel_Completed", "Não é possível cancelar um anúncio já concluído.");

    public static AdvertisementDomainException OnlyApprovedPausedCanComplete() => 
        new("Advertisement_Only_Approved_Paused_Can_Complete", "Apenas anúncios aprovados ou pausados podem ser concluídos.");

    public static AdvertisementDomainException ScheduledTourRequired() => 
        new("Advertisement_ScheduledTour_Required", "Identificador do passeio agendado é obrigatório.");

    public static AdvertisementDomainException CustomerRequired() => 
        new("Advertisement_Customer_Required", "Identificador do cliente é obrigatório.");

    public static AdvertisementDomainException SeatsInvalid() => 
        new("Advertisement_Seats_Invalid", "Número de assentos reservados deve ser maior que zero.");

    public static AdvertisementDomainException SeatsTooHigh() => 
        new("Advertisement_Seats_Too_High", "Número de assentos reservados não pode exceder 1000.");

    public static AdvertisementDomainException TotalInvalid() => 
        new("Advertisement_Total_Invalid", "Preço total deve ser maior que zero.");

    public static AdvertisementDomainException TotalTooHigh() => 
        new("Advertisement_Total_Too_High", "Preço total não pode exceder R$ 1.000.000,00.");

    public static AdvertisementDomainException NotesTooLong() => 
        new("Advertisement_Notes_Too_Long", "Observações não podem exceder 1000 caracteres.");
}