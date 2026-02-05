using NautiHub.Core.DomainObjects;

namespace NautiHub.Domain.Exceptions;

/// <summary>
/// Exceções de domínio específicas para a entidade Payment
/// </summary>
public class PaymentDomainException : DomainException
{
    public string MessageKey { get; }

    public PaymentDomainException(string messageKey, string defaultMessage) 
        : base(defaultMessage)
    {
        MessageKey = messageKey;
    }

    public PaymentDomainException(string messageKey, string defaultMessage, Exception innerException) 
        : base(defaultMessage, innerException)
    {
        MessageKey = messageKey;
    }

    // Métodos fáceis para criar exceções específicas
    public static PaymentDomainException NotFound() => 
        new("Payment_Not_Found", "Pagamento não encontrado");

    public static PaymentDomainException ValueMustBePositive() => 
        new("Validation_Payment_Value_Greater_Zero", "Payment value must be positive");

    public static PaymentDomainException DescriptionRequired() => 
        new("Validation_Payment_Description_Required", "Description is required");

    public static PaymentDomainException RefundValuePositive() => 
        new("Validation_Refund_Value_Greater_Zero", "Refund value must be greater than 0");

    public static PaymentDomainException RefundExceedsNetValue() => 
        new("Validation_Refund_Value_Greater_Zero", "Refund value cannot exceed net value");

    public static PaymentDomainException CannotDeletePaid() => 
        new("Payment_Cannot_Delete_Paid", "Cannot delete a paid payment");

    public static PaymentDomainException CannotConfirmWithStatus() => 
        new("Payment_Get_Error", "Cannot confirm payment with current status");

    public static PaymentDomainException CannotMarkAsOverdueWithStatus() => 
        new("Payment_Get_Error", "Cannot mark as overdue with current status");

    public static PaymentDomainException CannotRefundWithStatus() => 
        new("Payment_Refund_Not_Allowed", "Cannot refund payment with current status");
}