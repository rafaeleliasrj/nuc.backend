using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ScheduledTourUpdate;

/// <summary>
/// Feature para atualização de passeio agendado
/// </summary>
public class UpdateScheduledTourFeature : Feature<FeatureResponse<ScheduledTourResponse>>
{
    /// <summary>
    /// Identificador do passeio
    /// </summary>
    public Guid TourId { get; init; }

    /// <summary>
    /// Request de atualização de passeio agendado
    /// </summary>
    public UpdateScheduledTourRequest Data { get; init; }
}