using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ScheduledTourDelete;

/// <summary>
/// Feature para exclusão de passeio agendado
/// </summary>
public class DeleteScheduledTourFeature : Feature<FeatureResponse<bool>>
{
    /// <summary>
    /// Identificador do passeio
    /// </summary>
    public Guid TourId { get; init; }
}