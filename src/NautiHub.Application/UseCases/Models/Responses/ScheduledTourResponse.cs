using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response de passeio agendado
/// </summary>
public class ScheduledTourResponse
{
    /// <summary>
    /// Identificador do passeio
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador do usuário proprietário do barco
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Identificador do barco
    /// </summary>
    public Guid BoatId { get; set; }

    /// <summary>
    /// Data do passeio
    /// </summary>
    public DateOnly TourDate { get; set; }

    /// <summary>
    /// Hora de início do passeio
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// Hora de término do passeio
    /// </summary>
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// Número de assentos disponíveis
    /// </summary>
    public int AvailableSeats { get; set; }

    /// <summary>
    /// Status atual do passeio
    /// </summary>
    public ScheduledTourStatus Status { get; set; }

    /// <summary>
    /// Observações adicionais sobre o passeio
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Indica se o passeio está ativo
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Indica se o passeio pode ser editado
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// Data de criação do passeio
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}