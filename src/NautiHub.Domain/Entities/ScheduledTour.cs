using NautiHub.Core.DomainObjects;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Entities;

/// <summary>
/// Entidade que representa um passeio agendado no sistema de aluguel de barcos.
/// </summary>
public class ScheduledTour : Entity, IAggregateRoot
{
    /// <summary>
    /// Construtor padrão para EF Core.
    /// </summary>
    public ScheduledTour() { }

    /// <summary>
    /// Construtor com dados iniciais do passeio agendado.
    /// </summary>
    /// <param name="userId">Identificador do usuário proprietário do barco.</param>
    /// <param name="boatId">Identificador do barco.</param>
    /// <param name="tourDate">Data do passeio.</param>
    /// <param name="startTime">Hora de início do passeio.</param>
    /// <param name="endTime">Hora de término do passeio.</param>
    /// <param name="availableSeats">Número de assentos disponíveis.</param>
    /// <param name="status">Status inicial do passeio.</param>
    /// <param name="notes">Observações adicionais.</param>
    public ScheduledTour(
        Guid userId,
        Guid boatId,
        DateOnly tourDate,
        TimeOnly startTime,
        TimeOnly endTime,
        int availableSeats,
        ScheduledTourStatus status = ScheduledTourStatus.Scheduled,
        string? notes = null)
    {
        UserId = userId;
        BoatId = boatId;
        TourDate = tourDate;
        StartTime = startTime;
        EndTime = endTime;
        AvailableSeats = availableSeats;
        Status = status;
        Notes = notes;
        
        Validate();
    }

    /// <summary>
    /// Identificador do barco.
    /// </summary>
    public Guid BoatId { get; private set; }

    /// <summary>
    /// Relacionamento com o barco.
    /// </summary>
    public virtual Boat Boat { get; private set; } = null!;

    /// <summary>
    /// Data do passeio.
    /// </summary>
    public DateOnly TourDate { get; private set; }

    /// <summary>
    /// Hora de início do passeio.
    /// </summary>
    public TimeOnly StartTime { get; private set; }

    /// <summary>
    /// Hora de término do passeio.
    /// </summary>
    public TimeOnly EndTime { get; private set; }

    /// <summary>
    /// Número de assentos disponíveis no passeio.
    /// </summary>
    public int AvailableSeats { get; private set; }

    /// <summary>
    /// Status atual do passeio.
    /// </summary>
    public ScheduledTourStatus Status { get; private set; }

    /// <summary>
    /// Observações adicionais sobre o passeio.
    /// </summary>
    public string? Notes { get; private set; }    

    /// <summary>
    /// Inicia o passeio.
    /// </summary>
    public void Start()
    {
        if (Status != ScheduledTourStatus.Scheduled)
            throw ScheduledTourDomainException.OnlyScheduledCanStart();
        
        Status = ScheduledTourStatus.InProgress;
    }

    /// <summary>
    /// Conclui o passeio.
    /// </summary>
    public void Complete()
    {
        if (Status != ScheduledTourStatus.InProgress)
            throw ScheduledTourDomainException.OnlyStartedCanComplete();
        
        Status = ScheduledTourStatus.Completed;
    }

    /// <summary>
    /// Cancela o passeio.
    /// </summary>
    public void Cancel()
    {
        if (Status == ScheduledTourStatus.Cancelled)
            throw ScheduledTourDomainException.AlreadyCanceled();
        
        if (Status == ScheduledTourStatus.Completed)
            throw ScheduledTourDomainException.CannotCancelCompleted();
        
        Status = ScheduledTourStatus.Cancelled;
    }

    /// <summary>
    /// Suspende temporariamente o passeio.
    /// </summary>
    public void Suspend()
    {
        if (Status != ScheduledTourStatus.Scheduled && Status != ScheduledTourStatus.InProgress)
            throw ScheduledTourDomainException.OnlyScheduledStartedCanSuspend();
        
        if (Status == ScheduledTourStatus.Suspended)
            throw ScheduledTourDomainException.AlreadySuspended();
        
        Status = ScheduledTourStatus.Suspended;
    }

    /// <summary>
    /// Reativa um passeio suspenso.
    /// </summary>
    public void Reactivate()
    {
        if (Status != ScheduledTourStatus.Suspended)
            throw ScheduledTourDomainException.OnlySuspendedCanReactive();
        
        Status = ScheduledTourStatus.Scheduled;
    }

    /// <summary>
    /// Atualiza o número de assentos disponíveis.
    /// </summary>
    /// <param name="availableSeats">Novo número de assentos disponíveis.</param>
    public void UpdateAvailableSeats(int availableSeats)
    {
        if (availableSeats < 0)
            throw ScheduledTourDomainException.SeatsNegative();
        
        AvailableSeats = availableSeats;
    }

    /// <summary>
    /// Atualiza as observações do passeio.
    /// </summary>
    /// <param name="notes">Novas observações.</param>
    public void UpdateNotes(string? notes)
    {
        if (notes != null && notes.Length > 1000)
            throw ScheduledTourDomainException.NotesTooLong();
        
        Notes = notes;
    }

    /// <summary>
    /// Atualiza o horário do passeio.
    /// </summary>
    /// <param name="startTime">Nova hora de início.</param>
    /// <param name="endTime">Nova hora de término.</param>
    public void UpdateSchedule(TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
            throw ScheduledTourDomainException.StartAfterEnd();
        
        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>
    /// Verifica se o passeio está ativo.
    /// </summary>
    public bool IsActive => Status == ScheduledTourStatus.Scheduled || Status == ScheduledTourStatus.InProgress;

    /// <summary>
    /// Verifica se o passeio pode ser editado.
    /// </summary>
    public bool CanEdit => Status == ScheduledTourStatus.Scheduled;

    /// <summary>
    /// Valida as regras de negócio do passeio agendado.
    /// </summary>
    private void Validate()
    {
        if (BoatId == Guid.Empty)
            throw ScheduledTourDomainException.BoatRequired();

        if (TourDate < DateOnly.FromDateTime(DateTime.Today))
            throw ScheduledTourDomainException.DatePast();

        if (StartTime >= EndTime)
            throw ScheduledTourDomainException.StartAfterEnd();

        if (AvailableSeats < 0)
            throw ScheduledTourDomainException.SeatsNegative();

        if (AvailableSeats > 1000)
            throw ScheduledTourDomainException.SeatsTooHigh();

        if (Notes != null && Notes.Length > 1000)
            throw ScheduledTourDomainException.NotesTooLong();
    }
}