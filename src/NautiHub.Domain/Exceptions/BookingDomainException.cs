using NautiHub.Core.DomainObjects;

namespace NautiHub.Domain.Exceptions;

/// <summary>
/// Exceções de domínio específicas para a entidade Booking
/// </summary>
public class BookingDomainException : DomainException
{
    public string MessageKey { get; }

    public BookingDomainException(string messageKey, string defaultMessage) 
        : base(defaultMessage)
    {
        MessageKey = messageKey;
    }

    public BookingDomainException(string messageKey, string defaultMessage, Exception innerException) 
        : base(defaultMessage, innerException)
    {
        MessageKey = messageKey;
    }

    // Métodos fáceis para criar exceções específicas
    public static BookingDomainException StartDatePast() => 
        new("Validation_Tour_Date_Past", "Start date cannot be in past");

    public static BookingDomainException EndDateAfterStart() => 
        new("Validation_End_Time_After_Start", "End date must be after start date");

    public static BookingDomainException TotalPassengersPositive() => 
        new("Validation_Available_Seats_Greater_Zero", "Total passengers must be positive");

    public static BookingDomainException DailyPricePositive() => 
        new("Validation_Payment_Value_Greater_Zero", "Daily price must be positive");

    public static BookingDomainException TotalPricePositive() => 
        new("Validation_Payment_Value_Greater_Zero", "Total price must be positive");

    public static BookingDomainException SecurityDepositNegative() => 
        new("Validation_Refund_Value_Greater_Zero", "Security deposit cannot be negative");

    public static BookingDomainException PaymentMethodRequired() => 
        new("Validation_Payment_Method_Invalid", "Payment method is required");

    public static BookingDomainException TransactionIdRequired() => 
        new("Validation_Payment_Document_Required", "Transaction ID is required");

    public static BookingDomainException CancellationReasonRequired() => 
        new("Validation_Refund_Reason_Required", "Cancellation reason is required");

    public static BookingDomainException CancellationPercentageInvalid() => 
        new("Validation_Refund_Value_Greater_Zero", "Cancellation percentage must be between 0 and 100");

    public static BookingDomainException CannotConfirmWithStatus() => 
        new("Error_Bad_Request_Message", "Cannot confirm booking with current status");

    public static BookingDomainException CannotMarkAsPaidWithStatus() => 
        new("Error_Bad_Request_Message", "Cannot mark as paid with current status");

    public static BookingDomainException CannotStartTripWithStatus() => 
        new("Error_Bad_Request_Message", "Cannot start trip with current status");

    public static BookingDomainException CannotCompleteTripWithStatus() => 
        new("Error_Bad_Request_Message", "Cannot complete trip with current status");

    public static BookingDomainException CannotCancelWithStatus() => 
        new("Error_Bad_Request_Message", "Cannot cancel booking with current status");

    public static BookingDomainException CannotUpdatePassengerInfoWithStatus() => 
        new("Error_Bad_Request_Message", "Cannot update passenger info with current status");
}