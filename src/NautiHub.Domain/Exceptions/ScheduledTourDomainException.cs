using NautiHub.Core.DomainObjects;

namespace NautiHub.Domain.Exceptions;

/// <summary>
/// Exceções de domínio específicas para a entidade ScheduledTour
/// </summary>
public class ScheduledTourDomainException : DomainException
{
    public string MessageKey { get; }

    public ScheduledTourDomainException(string messageKey, string defaultMessage) 
        : base(defaultMessage)
    {
        MessageKey = messageKey;
    }

    public ScheduledTourDomainException(string messageKey, string defaultMessage, Exception innerException) 
        : base(defaultMessage, innerException)
    {
        MessageKey = messageKey;
    }

    // Métodos fáceis para criar exceções específicas
    public static ScheduledTourDomainException NotFound() => 
        new("ScheduledTour_Not_Found", "Passeio agendado {0} não encontrado");

    public static ScheduledTourDomainException Conflict() => 
        new("ScheduledTour_Conflict", "Já existe um passeio agendado para este horário");

    public static ScheduledTourDomainException CannotDeleteCompleted() => 
        new("ScheduledTour_Cannot_Delete_Completed", "Não é possível excluir um passeio já concluído");

    public static ScheduledTourDomainException CannotEditStatus() => 
        new("ScheduledTour_Cannot_Edit_Status", "Este passeio não pode ser editado no status atual");

    public static ScheduledTourDomainException StartAfterEnd() => 
        new("ScheduledTour_Start_After_End", "Hora de início deve ser anterior à hora de término");

    public static ScheduledTourDomainException OnlyScheduledCanStart() => 
        new("ScheduledTour_Only_Scheduled_Can_Start", "Apenas passeios agendados podem ser iniciados.");

    public static ScheduledTourDomainException OnlyStartedCanComplete() => 
        new("ScheduledTour_Only_Started_Can_Complete", "Apenas passeios em andamento podem ser concluídos.");

    public static ScheduledTourDomainException AlreadyCanceled() => 
        new("ScheduledTour_Already_Canceled", "Passeio já está cancelado.");

    public static ScheduledTourDomainException CannotCancelCompleted() => 
        new("ScheduledTour_Cannot_Cancel_Completed", "Não é possível cancelar um passeio já concluído.");

    public static ScheduledTourDomainException OnlyScheduledStartedCanSuspend() => 
        new("ScheduledTour_Only_Scheduled_Started_Can_Suspend", "Apenas passeios agendados ou em andamento podem ser suspensos.");

    public static ScheduledTourDomainException AlreadySuspended() => 
        new("ScheduledTour_Already_Suspended", "Passeio já está suspenso.");

    public static ScheduledTourDomainException OnlySuspendedCanReactive() => 
        new("ScheduledTour_Only_Suspended_Can_Reactive", "Apenas passeios suspensos podem ser reativados.");

    public static ScheduledTourDomainException SeatsNegative() => 
        new("ScheduledTour_Seats_Negative", "Número de assentos disponíveis não pode ser negativo.");

    public static ScheduledTourDomainException NotesTooLong() => 
        new("ScheduledTour_Notes_Too_Long", "Observações não podem exceder 1000 caracteres.");

    public static ScheduledTourDomainException BoatRequired() => 
        new("ScheduledTour_Boat_Required", "Identificador do barco é obrigatório.");

    public static ScheduledTourDomainException DatePast() => 
        new("ScheduledTour_Date_Past", "Data do passeio não pode ser anterior à data atual.");

    public static ScheduledTourDomainException SeatsTooHigh() => 
        new("ScheduledTour_Seats_Too_High", "Número de assentos disponíveis não pode exceder 1000.");

    public static ScheduledTourDomainException StatusNotImplemented() => 
        new("ScheduledTour_Status_Not_Implemented", "Atualização de status não implementada.");
}