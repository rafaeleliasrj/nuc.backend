using NautiHub.Core.DomainObjects;
using NautiHub.Domain.ValueObjects;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Entities;

/// <summary>
/// Representa um pagamento processado através do gateway Asaas
/// </summary>
public class Payment : Entity, IAggregateRoot
{
    public string AsaasPaymentId { get; private set; }
    public Guid BookingId { get; private set; }
    public Booking Booking { get; private set; }
    public decimal Value { get; private set; }
    public decimal NetValue { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string InvoiceUrl { get; private set; }
    public string BankSlipUrl { get; private set; }
    public string TransactionReceiptUrl { get; private set; }
    public string PixQrCode { get; private set; }
    public string PixEncodedImage { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime? ConfirmedDate { get; private set; }
    public DateTime? PaymentDate { get; private set; }
    public DateTime? CreditDate { get; private set; }
    public string Description { get; private set; }
    public string ExternalReference { get; private set; }
    public string BillingType { get; private set; }
    public CreditCardInfo CreditCardInfo { get; private set; }
    public List<PaymentSplit> Splits { get; private set; }

    // Construtor privado para EF Core
    private Payment() 
    {
        Splits = new List<PaymentSplit>();
    }

    public Payment(
        Guid bookingId,
        decimal value,
        PaymentMethod method,
        string description,
        string externalReference = null)
    {
        if (value <= 0)
            throw PaymentDomainException.ValueMustBePositive();

        if (string.IsNullOrWhiteSpace(description))
            throw PaymentDomainException.DescriptionRequired();

        BookingId = bookingId;
        Value = value;
        NetValue = value; // Inicialmente igual, será atualizado após resposta do Asaas
        Method = method;
        Status = PaymentStatus.Pending;
        Description = description;
        ExternalReference = externalReference;
        BillingType = method.ToString().ToUpperInvariant();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Splits = new List<PaymentSplit>();
    }

    // Propriedades calculadas
    public bool IsPending => Status == PaymentStatus.Pending;
    public bool IsConfirmed => Status == PaymentStatus.Paid;
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow.Date && Status == PaymentStatus.Pending;
    public bool CanBeRefunded => Status == PaymentStatus.Paid && !IsDeleted;

    // Métodos de domínio
    public void UpdateAsaasInfo(
        string asaasPaymentId,
        decimal netValue,
        string invoiceUrl = null,
        string bankSlipUrl = null,
        string transactionReceiptUrl = null,
        string pixQrCode = null,
        string pixEncodedImage = null,
        DateTime? dueDate = null,
        CreditCardInfo creditCardInfo = null,
        List<PaymentSplit> splits = null)
    {
        if (!string.IsNullOrWhiteSpace(asaasPaymentId))
            AsaasPaymentId = asaasPaymentId;

        if (netValue > 0)
            NetValue = netValue;

        if (!string.IsNullOrWhiteSpace(invoiceUrl))
            InvoiceUrl = invoiceUrl;

        if (!string.IsNullOrWhiteSpace(bankSlipUrl))
            BankSlipUrl = bankSlipUrl;

        if (!string.IsNullOrWhiteSpace(transactionReceiptUrl))
            TransactionReceiptUrl = transactionReceiptUrl;

        if (!string.IsNullOrWhiteSpace(pixQrCode))
            PixQrCode = pixQrCode;

        if (!string.IsNullOrWhiteSpace(pixEncodedImage))
            PixEncodedImage = pixEncodedImage;

        if (dueDate.HasValue)
            DueDate = dueDate;

        if (creditCardInfo != null)
            CreditCardInfo = creditCardInfo;

        if (splits != null && splits.Any())
        {
            Splits.Clear();
            Splits.AddRange(splits);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void Confirm(DateTime? confirmedDate = null)
    {
        if (Status != PaymentStatus.Pending)
            throw PaymentDomainException.CannotConfirmWithStatus();

        Status = PaymentStatus.Paid;
        ConfirmedDate = confirmedDate ?? DateTime.UtcNow;
        PaymentDate = confirmedDate ?? DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsOverdue()
    {
        if (Status != PaymentStatus.Pending)
            throw PaymentDomainException.CannotMarkAsOverdueWithStatus();

        Status = PaymentStatus.Failed; // Usamos Failed para representar vencido
        UpdatedAt = DateTime.UtcNow;
    }

    public void Refund(decimal refundValue, string reason = null)
    {
        if (!CanBeRefunded)
            throw PaymentDomainException.CannotRefundWithStatus();

        if (refundValue <= 0)
            throw PaymentDomainException.RefundValuePositive();

        if (refundValue > NetValue)
            throw PaymentDomainException.RefundExceedsNetValue();

        if (refundValue >= NetValue)
        {
            Status = PaymentStatus.Refunded;
        }
        else
        {
            Status = PaymentStatus.PartiallyRefunded;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(PaymentStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        // Atualizar datas baseadas no status
        switch (newStatus)
        {
            case PaymentStatus.Paid:
                if (!ConfirmedDate.HasValue)
                    ConfirmedDate = DateTime.UtcNow;
                if (!PaymentDate.HasValue)
                    PaymentDate = DateTime.UtcNow;
                break;
        }
    }

    public void Delete()
    {
        if (Status == PaymentStatus.Paid)
            throw PaymentDomainException.CannotDeletePaid();

        MarkAsDeleted();
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddSplit(PaymentSplit split)
    {
        if (split == null)
            throw new ArgumentNullException(nameof(split));

        Splits.Add(split);
        UpdatedAt = DateTime.UtcNow;
    }

    public decimal GetTotalSplitAmount()
    {
        return Splits.Sum(s => (s.FixedValue ?? 0) + (s.PercentualValue.HasValue ? s.PercentualValue.Value * Value / 100 : 0));
    }
}