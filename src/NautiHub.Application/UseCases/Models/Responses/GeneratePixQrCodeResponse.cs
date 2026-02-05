namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response da feature de geração de QR Code Pix
/// </summary>
public class GeneratePixQrCodeResponse
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// QR Code em base64 (imagem)
    /// </summary>
    public string QrCodeImage { get; set; }

    /// <summary>
    /// Payload do QR Code (para copiar e colar)
    /// </summary>
    public string Payload { get; set; }

    /// <summary>
    /// Data de expiração do QR Code
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Valor do pagamento
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Status atual do pagamento
    /// </summary>
    public string Status { get; set; }
}