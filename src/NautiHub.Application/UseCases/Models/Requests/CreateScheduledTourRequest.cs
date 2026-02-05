using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para criação de passeio agendado
/// </summary>
public class CreateScheduledTourRequest
{
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
    /// Status inicial do passeio
    /// </summary>
    public ScheduledTourStatus Status { get; set; } = ScheduledTourStatus.Scheduled;

    /// <summary>
    /// Observações adicionais
    /// </summary>
    public string? Notes { get; set; }
}