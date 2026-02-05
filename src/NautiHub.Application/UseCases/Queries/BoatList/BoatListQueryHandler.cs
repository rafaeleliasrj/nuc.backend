using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Application.Services;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Models;
using NautiHub.Core.Messages.Queries;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;

namespace NautiHub.Application.UseCases.Queries.BoatList;

public class BoatListQueryHandler(
    ILogger<BoatListQueryHandler> logger,
    IBoatRepository boatRepository,
    IMappingService mappingService
)
    : QueryHandler,
        IRequestHandler<
            BoatListQuery,
            QueryResponse<ListPaginationResponse<BoatListResponse>>
        >
{
    private readonly ILogger<BoatListQueryHandler> _logger = logger;
    private readonly IBoatRepository _boatRepository = boatRepository;
    private readonly IMappingService _mappingService = mappingService;

    public async Task<QueryResponse<ListPaginationResponse<BoatListResponse>>> Handle(
        BoatListQuery request,
        CancellationToken cancellationToken
    )
    {
        ListPaginationResponse<Boat> list = await _boatRepository.ListAsync(
            request.Filter.Search,
            request.Filter.CreatedAtStart,
            request.Filter.CreatedAtEnd,
            request.Filter.UpdatedAtStart,
            request.Filter.UpdatedAtEnd,
            request.Filter.Page,
            request.Filter.PerPage,
            request.Filter.OrderBy
        );

        var result = new ListPaginationResponse<BoatListResponse>()
        {
            CurrentPage = list.CurrentPage,
            PageSize = list.PageSize,
            PageCount = list.PageCount,
            RowCount = list.RowCount,
            Data = _mappingService.MapToBoatListResponse(list.Data)
        };

        return new QueryResponse<ListPaginationResponse<BoatListResponse>>(result);
    }
}
