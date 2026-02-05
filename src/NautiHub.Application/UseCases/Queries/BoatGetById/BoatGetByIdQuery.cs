using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Queries;

namespace NautiHub.Application.UseCases.Queries.BoatGetById;

public class BoatGetByIdQuery : Query<QueryResponse<BoatResponse>>
{
    public Guid BoatId { get; set; }
}
