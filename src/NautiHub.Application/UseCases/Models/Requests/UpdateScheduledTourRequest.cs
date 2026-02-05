using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para atualização de passeio agendado
/// </summary>
public class UpdateScheduledTourRequest
{
    /// <summary>
    /// Data atualizada do passeio
    /// </summary>
    public DateOnly? TourDate { get; set; }

    /// <summary>
    /// Hora atualizada de início do passeio
    /// </summary>
    public TimeOnly? StartTime { get; set; }

    /// <summary>
    /// Hora atualizada de término do passeio
    /// </summary>
    public TimeOnly? EndTime { get; set; }

    /// <summary>
    /// Número atualizado de assentos disponíveis
    /// </summary>
    public int? AvailableSeats { get; set; }

    /// <summary>
    /// Observações atualizadas
    /// </summary>
    public string? Notes { get; set; }
}