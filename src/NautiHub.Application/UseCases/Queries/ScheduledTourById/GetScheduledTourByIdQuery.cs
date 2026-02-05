using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ScheduledTourById;

/// <summary>
/// Query para buscar passeio agendado por ID
/// </summary>
public class GetScheduledTourByIdQuery(Guid id) : Query<QueryResponse<ScheduledTourResponse>>
{
    /// <summary>
    /// ID do passeio
    /// </summary>
    public Guid Id { get; set; } = id;
}