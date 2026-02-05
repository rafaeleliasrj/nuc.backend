using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ScheduledTourCreate;

/// <summary>
/// Feature para criação de passeio agendado
/// </summary>
public class CreateScheduledTourFeature : Feature<FeatureResponse<ScheduledTourResponse>>
{
    /// <summary>
    /// Request de criação de passeio agendado
    /// </summary>
    public CreateScheduledTourRequest Data { get; init; }
}