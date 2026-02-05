namespace NautiHub.Domain.ValueObjects;

/// <summary>
/// Informações do cartão de crédito mascaradas para armazenamento seguro
/// </summary>
public class CreditCardInfo
{
    public string LastFourDigits { get; private set; }
    public string Brand { get; private set; }
    public string HolderName { get; private set; }
    public string Token { get; private set; }
    public int ExpiryMonth { get; private set; }
    public int ExpiryYear { get; private set; }

    // Construtor privado para EF Core
    private CreditCardInfo() { }

    public CreditCardInfo(
        string lastFourDigits,
        string brand,
        string holderName,
        string token,
        int expiryMonth,
        int expiryYear)
    {
        if (string.IsNullOrWhiteSpace(lastFourDigits) || lastFourDigits.Length != 4)
            throw new ArgumentException("Last four digits must be exactly 4 characters", nameof(lastFourDigits));

        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand is required", nameof(brand));

        if (string.IsNullOrWhiteSpace(holderName))
            throw new ArgumentException("Holder name is required", nameof(holderName));

        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token is required", nameof(token));

        if (expiryMonth < 1 || expiryMonth > 12)
            throw new ArgumentException("Expiry month must be between 1 and 12", nameof(expiryMonth));

        if (expiryYear < DateTime.UtcNow.Year || expiryYear > DateTime.UtcNow.Year + 10)
            throw new ArgumentException("Invalid expiry year", nameof(expiryYear));

        LastFourDigits = lastFourDigits;
        Brand = brand.ToUpperInvariant();
        HolderName = holderName.Trim();
        Token = token;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
    }

    /// <summary>
    /// Cria CreditCardInfo a partir de dados mascarados do Asaas
    /// </summary>
    public static CreditCardInfo FromAsaasResponse(
        string creditCardNumber,
        string creditCardBrand,
        string creditCardToken)
    {
        var lastFour = creditCardNumber?.Length >= 4 
            ? creditCardNumber[^4..] 
            : creditCardNumber ?? "****";

        return new CreditCardInfo(
            lastFour,
            creditCardBrand ?? "UNKNOWN",
            null, // Holder name não é retornado por segurança
            creditCardToken ?? string.Empty,
            0, // Expiry não é retornado por segurança
            0
        );
    }

    /// <summary>
    /// Verifica se o cartão está expirado
    /// </summary>
    public bool IsExpired()
    {
        var expiryDate = new DateTime(ExpiryYear, ExpiryMonth, 1).AddMonths(1).AddDays(-1);
        return DateTime.UtcNow > expiryDate;
    }

    /// <summary>
    /// Retorna o número do cartão mascarado para exibição
    /// </summary>
    public string GetMaskedNumber()
    {
        return $"****-****-****-{LastFourDigits}";
    }

    /// <summary>
    /// Retorna a data de validade no formato MM/AA
    /// </summary>
    public string GetFormattedExpiry()
    {
        return $"{ExpiryMonth:D2}/{(ExpiryYear % 100):D2}";
    }
}