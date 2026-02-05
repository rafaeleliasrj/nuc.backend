namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response da feature de geração de boleto
/// </summary>
public class GenerateBoletoResponse
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// URL do boleto (para acesso online)
    /// </summary>
    public string BankSlipUrl { get; set; }

    /// <summary>
    /// Linha digitável
    /// </summary>
    public string DigitableLine { get; set; }

    /// <summary>
    /// Código de barras
    /// </summary>
    public string BarcodeNumber { get; set; }

    /// <summary>
    /// Nosso número
    /// </summary>
    public string NossoNumero { get; set; }

    /// <summary>
    /// Data de vencimento
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Valor do boleto
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Status atual do pagamento
    /// </summary>
    public string Status { get; set; }
}