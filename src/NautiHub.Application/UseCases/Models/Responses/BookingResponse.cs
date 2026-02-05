using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response com dados da reserva para API.
/// </summary>
public class BookingResponse
{
    /// <summary>
    /// Identificador único da reserva.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Número único da reserva.
    /// </summary>
    public string BookingNumber { get; set; } = string.Empty;

    /// <summary>
    /// ID do barco reservado.
    /// </summary>
    public Guid BoatId { get; set; }

    /// <summary>
    /// ID do hóspede que fez a reserva.
    /// </summary>
    public Guid GuestId { get; set; }

    /// <summary>
    /// ID do usuário (para compatibilidade).
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Tipo da reserva (Inteira ou por Assento).
    /// </summary>
    public BookingType Type { get; set; }

    /// <summary>
    /// Data de início da reserva.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Data de término da reserva.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Número total de passageiros.
    /// </summary>
    public int TotalPassengers { get; set; }

    /// <summary>
    /// Lista com nomes dos passageiros (JSON).
    /// </summary>
    public string PassengerNamesJson { get; set; } = string.Empty;

    /// <summary>
    /// Preço diário da reserva.
    /// </summary>
    public decimal DailyPrice { get; set; }

    /// <summary>
    /// Preço total da reserva.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Valor do depósito de segurança.
    /// </summary>
    public decimal SecurityDeposit { get; set; }

    /// <summary>
    /// Taxa de cancelamento (se aplicável).
    /// </summary>
    public decimal? CancellationFee { get; set; }

    /// <summary>
    /// Método de pagamento utilizado.
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Status do pagamento.
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// ID da transação.
    /// </summary>
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>
    /// Status da reserva.
    /// </summary>
    public BookingStatus Status { get; set; }

    /// <summary>
    /// Data de confirmação da reserva.
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// Data de cancelamento da reserva.
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// Motivo do cancelamento.
    /// </summary>
    public string CancellationReason { get; set; } = string.Empty;

    /// <summary>
    /// Horário real de início (check-in).
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// Horário real de término (check-out).
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// Quantidade de pagamentos associados à reserva.
    /// </summary>
    public int PaymentCount { get; set; }

    /// <summary>
    /// Valor total já pago.
    /// </summary>
    public decimal TotalPaid { get; set; }

    /// <summary>
    /// Data de criação da reserva.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}