namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Splits de pagamento
/// </summary>
public class PaymentSplitRequest
{
    /// <summary>
    /// Wallet ID do recebedor
    /// </summary>
    public string WalletId { get; set; }

    /// <summary>
    /// Valor fixo do split
    /// </summary>
    public decimal? FixedValue { get; set; }

    /// <summary>
    /// Valor percentual do split
    /// </summary>
    public decimal? PercentualValue { get; set; }

    /// <summary>
    /// Descrição do split
    /// </summary>
    public string Description { get; set; }
}