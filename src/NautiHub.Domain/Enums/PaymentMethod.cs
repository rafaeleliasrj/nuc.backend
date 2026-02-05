namespace NautiHub.Domain.Enums;

/// <summary>
/// Métodos de pagamento suportados pela plataforma
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Indefinido
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Boleto bancário
    /// </summary>
    Boleto = 1,
    
    /// <summary>
    /// Cartão de crédito
    /// </summary>
    CreditCard = 2,
    
    /// <summary>
    /// Pix (QR Code dinâmico)
    /// </summary>
    Pix = 3,
    
    /// <summary>
    /// Dinheiro (confirmado manualmente)
    /// </summary>
    Cash = 4,
    
    /// <summary>
    /// Transferência bancária
    /// </summary>
    Transfer = 5    
}