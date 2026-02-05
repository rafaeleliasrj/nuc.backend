namespace NautiHub.Domain.ValueObjects;

/// <summary>
/// Representa uma divisão de pagamento (split) para múltiplos recebedores
/// </summary>
public class PaymentSplit
{
    public string WalletId { get; private set; }
    public decimal? FixedValue { get; private set; }
    public decimal? PercentualValue { get; private set; }
    public string Status { get; private set; }
    public string RefusalReason { get; private set; }
    public string ExternalReference { get; private set; }
    public string Description { get; private set; }

    // Construtor privado para EF Core
    private PaymentSplit() { }

    public PaymentSplit(
        string walletId,
        decimal? fixedValue = null,
        decimal? percentualValue = null,
        string externalReference = null,
        string description = null)
    {
        if (string.IsNullOrWhiteSpace(walletId))
            throw new ArgumentException("Wallet ID is required", nameof(walletId));

        if (fixedValue.HasValue && fixedValue.Value <= 0)
            throw new ArgumentException("Fixed value must be positive", nameof(fixedValue));

        if (percentualValue.HasValue && (percentualValue.Value <= 0 || percentualValue.Value > 100))
            throw new ArgumentException("Percentual value must be between 0 and 100", nameof(percentualValue));

        if (!fixedValue.HasValue && !percentualValue.HasValue)
            throw new ArgumentException("Either fixed value or percentual value must be specified");

        WalletId = walletId;
        FixedValue = fixedValue;
        PercentualValue = percentualValue;
        ExternalReference = externalReference;
        Description = description;
        Status = "PENDING";
    }

    /// <summary>
    /// Cria um split de valor fixo
    /// </summary>
    public static PaymentSplit Fixed(string walletId, decimal value, string externalReference = null, string description = null)
    {
        return new PaymentSplit(walletId, fixedValue: value, externalReference: externalReference, description: description);
    }

    /// <summary>
    /// Cria um split percentual
    /// </summary>
    public static PaymentSplit Percentual(string walletId, decimal percentage, string externalReference = null, string description = null)
    {
        return new PaymentSplit(walletId, percentualValue: percentage, externalReference: externalReference, description: description);
    }

    /// <summary>
    /// Marca o split como aprovado
    /// </summary>
    public void Approve()
    {
        Status = "APPROVED";
    }

    /// <summary>
    /// Marca o split como recusado com motivo
    /// </summary>
    public void Refuse(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Refusal reason is required", nameof(reason));

        Status = "REFUSED";
        RefusalReason = reason;
    }

    /// <summary>
    /// Calcula o valor do split baseado no valor total do pagamento
    /// </summary>
    public decimal CalculateValue(decimal totalPaymentValue)
    {
        if (FixedValue.HasValue)
            return FixedValue.Value;

        if (PercentualValue.HasValue)
            return totalPaymentValue * PercentualValue.Value / 100;

        return 0;
    }

    /// <summary>
    /// Verifica se o split está pendente
    /// </summary>
    public bool IsPending => Status == "PENDING";

    /// <summary>
    /// Verifica se o split foi aprovado
    /// </summary>
    public bool IsApproved => Status == "APPROVED";

    /// <summary>
    /// Verifica se o split foi recusado
    /// </summary>
    public bool IsRefused => Status == "REFUSED";
}