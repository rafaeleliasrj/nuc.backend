using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para atualizar status do passeio agendado
/// </summary>
public class UpdateScheduledTourStatusRequest
{
    /// <summary>
    /// Novo status do passeio
    /// </summary>
    public ScheduledTourStatus Status { get; set; }

    /// <summary>
    /// Justificativa da mudança de status
    /// </summary>
    public string? Reason { get; set; }
}