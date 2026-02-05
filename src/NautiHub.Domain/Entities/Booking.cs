using NautiHub.Core.DomainObjects;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Entities;

public class Booking : Entity, IAggregateRoot
{
    public string BookingNumber { get; private set; }

    // Participantes
    public Guid BoatId { get; private set; }
    public Boat Boat { get; private set; }

    public Guid GuestId { get; private set; }
    public User Guest { get; private set; }

    // Detalhes da reserva
    public BookingType Type { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    // Passageiros
    public int TotalPassengers { get; private set; }
    public string PassengerNamesJson { get; private set; }

    // Preços
    public decimal DailyPrice { get; private set; }
    public decimal TotalPrice { get; private set; }
    public decimal SecurityDeposit { get; private set; }
    public decimal? CancellationFee { get; private set; }

    // Pagamento
    public string PaymentMethod { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public string TransactionId { get; private set; }

    // Status
    public BookingStatus Status { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string CancellationReason { get; private set; }

    // Check-in/Check-out real
    public DateTime? ActualStartTime { get; private set; }
    public DateTime? ActualEndTime { get; private set; }

    // Pagamentos - Pertencem ao agregado Booking
    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    // Construtor privado para EF Core
    private Booking() { }

    public Booking(
        Guid boatId,
        Guid guestId,
        BookingType type,
        DateTime startDate,
        DateTime endDate,
        int totalPassengers,
        decimal dailyPrice,
        decimal totalPrice,
        decimal securityDeposit,
        List<string> passengerNames = null)
    {
        ValidateParameters(startDate, endDate, totalPassengers, dailyPrice, totalPrice, securityDeposit);

        BoatId = boatId;
        GuestId = guestId;
        UserId = guestId; // Para compatibilidade com IEntityUserControlAccess
        Type = type;
        StartDate = startDate.Date;
        EndDate = endDate.Date;
        TotalPassengers = totalPassengers;
        DailyPrice = dailyPrice;
        TotalPrice = totalPrice;
        SecurityDeposit = securityDeposit;
        PassengerNamesJson = passengerNames != null
            ? System.Text.Json.JsonSerializer.Serialize(passengerNames)
            : "[]";

        BookingNumber = GenerateBookingNumber();
        Status = BookingStatus.Pending;
        PaymentStatus = PaymentStatus.Pending;
    }

    private void ValidateParameters(
        DateTime startDate,
        DateTime endDate,
        int totalPassengers,
        decimal dailyPrice,
        decimal totalPrice,
        decimal securityDeposit)
    {
        if (startDate.Date < DateTime.UtcNow.Date)
            throw BookingDomainException.StartDatePast();

        if (endDate.Date <= startDate.Date)
            throw BookingDomainException.EndDateAfterStart();

        if (totalPassengers <= 0)
            throw BookingDomainException.TotalPassengersPositive();

        if (dailyPrice <= 0)
            throw BookingDomainException.DailyPricePositive();

        if (totalPrice <= 0)
            throw BookingDomainException.TotalPricePositive();

        if (securityDeposit < 0)
            throw BookingDomainException.SecurityDepositNegative();
    }

    private string GenerateBookingNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"BOOK-{timestamp}-{random}";
    }

    // Propriedades calculadas
    public int NumberOfDays => (EndDate.Date - StartDate.Date).Days;

    public List<string> PassengerNames =>
        string.IsNullOrEmpty(PassengerNamesJson)
            ? new List<string>()
            : System.Text.Json.JsonSerializer.Deserialize<List<string>>(PassengerNamesJson);

    public bool IsActive => Status == BookingStatus.Confirmed ||
                            Status == BookingStatus.Paid ||
                            Status == BookingStatus.InProgress;

    public bool CanCancel => Status == BookingStatus.Pending ||
                             Status == BookingStatus.Confirmed ||
                             Status == BookingStatus.Paid;

    public bool CanCheckIn => Status == BookingStatus.Confirmed ||
                              Status == BookingStatus.Paid;

    public bool CanCheckOut => Status == BookingStatus.InProgress;

    // Métodos de domínio
    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw BookingDomainException.CannotConfirmWithStatus();

        Status = BookingStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid(string paymentMethod, string transactionId)
    {
        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw BookingDomainException.PaymentMethodRequired();

        if (string.IsNullOrWhiteSpace(transactionId))
            throw BookingDomainException.TransactionIdRequired();

        if (Status != BookingStatus.Confirmed)
            throw BookingDomainException.CannotMarkAsPaidWithStatus();

        Status = BookingStatus.Paid;
        PaymentMethod = paymentMethod;
        TransactionId = transactionId;
        PaymentStatus = PaymentStatus.Paid;
    }

    public void StartTrip()
    {
        if (!CanCheckIn)
            throw BookingDomainException.CannotStartTripWithStatus();

        Status = BookingStatus.InProgress;
        ActualStartTime = DateTime.UtcNow;
    }

    public void CompleteTrip()
    {
        if (!CanCheckOut)
            throw BookingDomainException.CannotCompleteTripWithStatus();

        Status = BookingStatus.Completed;
        ActualEndTime = DateTime.UtcNow;
    }

    public void Cancel(string reason, decimal? cancellationFee = null)
    {
        if (!CanCancel)
            throw BookingDomainException.CannotCancelWithStatus();

        if (string.IsNullOrWhiteSpace(reason))
            throw BookingDomainException.CancellationReasonRequired();

        Status = BookingStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = reason;
        CancellationFee = cancellationFee;

        if (cancellationFee.HasValue && cancellationFee.Value > 0)
        {
            PaymentStatus = PaymentStatus.PartiallyRefunded;
        }
        else if (Status == BookingStatus.Paid)
        {
            PaymentStatus = PaymentStatus.Refunded;
        }
    }

    public void UpdatePassengerInfo(int totalPassengers, List<string> passengerNames)
    {
        if (totalPassengers <= 0)
            throw BookingDomainException.TotalPassengersPositive();

        if (Status != BookingStatus.Pending)
            throw BookingDomainException.CannotUpdatePassengerInfoWithStatus();

        TotalPassengers = totalPassengers;
        PassengerNamesJson = passengerNames != null
            ? System.Text.Json.JsonSerializer.Serialize(passengerNames)
            : "[]";
    }



    public bool IsTripCompleted() => Status == BookingStatus.Completed;

    public decimal CalculateCancellationFee(decimal cancellationPercentage)
    {
        if (cancellationPercentage < 0 || cancellationPercentage > 100)
            throw BookingDomainException.CancellationPercentageInvalid();

        if (Status == BookingStatus.Cancelled)
            return CancellationFee ?? 0;

        var daysUntilStart = (StartDate.Date - DateTime.UtcNow.Date).Days;

        // Política de cancelamento (exemplo)
        if (daysUntilStart >= 7)
            return 0; // Cancelamento gratuito com 7+ dias de antecedência
        else if (daysUntilStart >= 3)
            return TotalPrice * 0.25m; // 25% com 3-6 dias
        else if (daysUntilStart >= 1)
            return TotalPrice * 0.5m; // 50% com 1-2 dias
        else
            return TotalPrice; // 100% no mesmo dia
    }

    // Métodos de gerenciamento de Payments (parte do agregado)
    public void AddPayment(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        if (!_payments.Any(p => p.Id == payment.Id))
        {
            _payments.Add(payment);
        }
    }

    public Payment GetLatestPayment()
    {
        return _payments.OrderByDescending(p => p.CreatedAt).FirstOrDefault();
    }

    public List<Payment> GetPendingPayments()
    {
        return _payments.Where(p => p.Status == PaymentStatus.Pending).ToList();
    }

    public bool HasPaidPayments()
    {
        return _payments.Any(p => p.Status == PaymentStatus.Paid);
    }

    public decimal GetTotalPaid()
    {
        return _payments.Where(p => p.Status == PaymentStatus.Paid).Sum(p => p.Value);
    }

    
}