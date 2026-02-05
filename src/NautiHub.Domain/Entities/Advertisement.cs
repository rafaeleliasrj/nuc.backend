using NautiHub.Core.DomainObjects;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Entities;

/// <summary>
/// Entidade que representa um anúncio no sistema de aluguel de barcos.
/// </summary>
public class Advertisement : Entity, IAggregateRoot
{
    /// <summary>
    /// Construtor padrão para EF Core.
    /// </summary>
    public Advertisement() { }

    /// <summary>
    /// Construtor com dados iniciais do anúncio.
    /// </summary>
    /// <param name="scheduledTourId">Identificador do passeio agendado.</param>
    /// <param name="customerId">Identificador do cliente que criou o anúncio.</param>
    /// <param name="seatsBooked">Número de assentos reservados.</param>
    /// <param name="totalPrice">Preço total do anúncio.</param>
    /// <param name="status">Status inicial do anúncio.</param>
    /// <param name="paymentStatus">Status do pagamento.</param>
    /// <param name="notes">Observações adicionais.</param>
    public Advertisement(
        Guid scheduledTourId,
        Guid customerId,
        int seatsBooked,
        decimal totalPrice,
        AdvertisementStatus status = AdvertisementStatus.Pending,
        PaymentStatus paymentStatus = PaymentStatus.Pending,
        string? notes = null)
    {
        UserId = customerId;
        ScheduledTourId = scheduledTourId;
        CustomerId = customerId;
        SeatsBooked = seatsBooked;
        TotalPrice = totalPrice;
        Status = status;
        PaymentStatus = paymentStatus;
        Notes = notes;
        
        Validate();
    }

    /// <summary>
    /// Identificador do passeio agendado.
    /// </summary>
    public Guid ScheduledTourId { get; private set; }

    /// <summary>
    /// Identificador do cliente que criou o anúncio.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Número de assentos reservados no anúncio.
    /// </summary>
    public int SeatsBooked { get; private set; }

    /// <summary>
    /// Preço total do anúncio.
    /// </summary>
    public decimal TotalPrice { get; private set; }

    /// <summary>
    /// Status atual do anúncio.
    /// </summary>
    public AdvertisementStatus Status { get; private set; }

    /// <summary>
    /// Status do pagamento associado ao anúncio.
    /// </summary>
    public PaymentStatus PaymentStatus { get; private set; }

    /// <summary>
    /// Observações adicionais sobre o anúncio.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Relacionamento com o cliente.
    /// </summary>
    public virtual User Customer { get; private set; } = null!;

    /// <summary>
    /// Atualiza os dados do anúncio.
    /// </summary>
    /// <param name="seatsBooked">Novo número de assentos.</param>
    /// <param name="totalPrice">Novo preço total.</param>
    /// <param name="notes">Novas observações.</param>
    public void UpdateDetails(int seatsBooked, decimal totalPrice, string? notes = null)
    {
        SeatsBooked = seatsBooked;
        TotalPrice = totalPrice;
        Notes = notes;
        
        Validate();
    }

    /// <summary>
    /// Aprova o anúncio.
    /// </summary>
    public void Approve()
    {
        if (Status == AdvertisementStatus.Approved)
            throw AdvertisementDomainException.AlreadyApproved();
        
        if (Status == AdvertisementStatus.Cancelled)
            throw AdvertisementDomainException.CannotApproveCanceled();
        
        Status = AdvertisementStatus.Approved;
    }

    /// <summary>
    /// Rejeita o anúncio.
    /// </summary>
    public void Reject()
    {
        if (Status == AdvertisementStatus.Approved)
            throw AdvertisementDomainException.CannotRejectApproved();
        
        if (Status == AdvertisementStatus.Cancelled)
            throw AdvertisementDomainException.CannotRejectCanceled();
        
        Status = AdvertisementStatus.Rejected;
    }

    /// <summary>
    /// Suspende temporariamente o anúncio.
    /// </summary>
    public void Suspend()
    {
        if (Status != AdvertisementStatus.Approved)
            throw AdvertisementDomainException.OnlyApprovedCanSuspend();
        
        Status = AdvertisementStatus.Suspended;
    }

    /// <summary>
    /// Reativa um anúncio suspenso.
    /// </summary>
    public void Reactivate()
    {
        if (Status != AdvertisementStatus.Suspended)
            throw AdvertisementDomainException.OnlySuspendedCanReactive();
        
        Status = AdvertisementStatus.Approved;
    }

    /// <summary>
    /// Pausa o anúncio.
    /// </summary>
    public void Pause()
    {
        if (Status != AdvertisementStatus.Approved)
            throw AdvertisementDomainException.OnlyApprovedCanPause();
        
        Status = AdvertisementStatus.Paused;
    }

    /// <summary>
    /// Despausa o anúncio.
    /// </summary>
    public void Unpause()
    {
        if (Status != AdvertisementStatus.Paused)
            throw AdvertisementDomainException.OnlyPausedCanUnpause();
        
        Status = AdvertisementStatus.Approved;
    }

    /// <summary>
    /// Cancela o anúncio.
    /// </summary>
    public void Cancel()
    {
        if (Status == AdvertisementStatus.Cancelled)
            throw AdvertisementDomainException.AlreadyCanceled();
        
        if (Status == AdvertisementStatus.Completed)
            throw AdvertisementDomainException.CannotCancelCompleted();
        
        Status = AdvertisementStatus.Cancelled;
    }

    /// <summary>
    /// Finaliza o anúncio.
    /// </summary>
    public void Complete()
    {
        if (Status != AdvertisementStatus.Approved && Status != AdvertisementStatus.Paused)
            throw AdvertisementDomainException.OnlyApprovedPausedCanComplete();
        
        Status = AdvertisementStatus.Completed;
    }

    /// <summary>
    /// Atualiza o status do pagamento.
    /// </summary>
    /// <param name="paymentStatus">Novo status do pagamento.</param>
    public void UpdatePaymentStatus(PaymentStatus paymentStatus)
    {
        PaymentStatus = paymentStatus;
    }

    /// <summary>
    /// Verifica se o anúncio pode ser editado.
    /// </summary>
    public bool CanEdit => Status == AdvertisementStatus.Pending;

    /// <summary>
    /// Verifica se o anúncio está ativo para visualização.
    /// </summary>
    public bool IsActive => Status == AdvertisementStatus.Approved;

    /// <summary>
    /// Verifica se o pagamento está pendente.
    /// </summary>
    public bool IsPaymentPending => PaymentStatus == PaymentStatus.Pending;

    /// <summary>
    /// Verifica se o pagamento está confirmado.
    /// </summary>
    public bool IsPaymentConfirmed => PaymentStatus == PaymentStatus.Paid;

    /// <summary>
    /// Valida as regras de negócio do anúncio.
    /// </summary>
    private void Validate()
    {
        if (ScheduledTourId == Guid.Empty)
            throw AdvertisementDomainException.ScheduledTourRequired();

        if (CustomerId == Guid.Empty)
            throw AdvertisementDomainException.CustomerRequired();

        if (SeatsBooked <= 0)
            throw AdvertisementDomainException.SeatsInvalid();

        if (SeatsBooked > 1000)
            throw AdvertisementDomainException.SeatsTooHigh();

        if (TotalPrice <= 0)
            throw AdvertisementDomainException.TotalInvalid();

        if (TotalPrice > 1000000)
            throw AdvertisementDomainException.TotalTooHigh();

        if (Notes != null && Notes.Length > 1000)
            throw AdvertisementDomainException.NotesTooLong();
    }
}