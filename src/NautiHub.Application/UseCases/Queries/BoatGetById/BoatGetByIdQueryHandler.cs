using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Application.Services;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.DomainObjects;
using NautiHub.Core.Messages.Queries;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Domain.Services.InfrastructureService.Cache;

namespace NautiHub.Application.UseCases.Queries.BoatGetById;

public class BoatGetByIdQueryHandler(
    ILogger<BoatGetByIdQueryHandler> logger,
    IBoatRepository boatRepository,
    ICacheService cacheService,
    INautiHubIdentity nautiHubIdentity,
    IMappingService mappingService
    )
        : QueryHandler,
        IRequestHandler<BoatGetByIdQuery, QueryResponse<BoatResponse>>
{
    private readonly ILogger<BoatGetByIdQueryHandler> _logger = logger;
    private readonly IBoatRepository _boatRepository = boatRepository;
    private readonly ICacheService _cacheService = cacheService;
    private readonly INautiHubIdentity _nautiHubIdentity = nautiHubIdentity;
    private readonly IMappingService _mappingService = mappingService;

    public async Task<QueryResponse<BoatResponse>> Handle(
        BoatGetByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var cacheKey = $"empresa-simples:{_nautiHubIdentity.UserId}:{request.BoatId}";

        BoatResponse? boatCache = await _cacheService.GetAsync<BoatResponse>(cacheKey);
        if (boatCache is not null)
            return new QueryResponse<BoatResponse>(boatCache);

        Boat? boat = await _boatRepository.GetByIdAsync(request.BoatId);

        if (boat == null)
            return new QueryResponse<BoatResponse>(default!);

        BoatResponse viewModel = _mappingService.MapToBoatResponse(boat);

        var tempoCacheEnv = Environment.GetEnvironmentVariable("CACHE_DURACAO_EM_SEGUNDOS");
        var tempoCache = int.TryParse(tempoCacheEnv, out var parsed) ? parsed : 600;

        await _cacheService.SaveAsync(
            cacheKey,
            viewModel,
            tempoCache
        );

        _logger.LogInformation(
            "[ObterEmpresaQueryHandler] - [EmpresaId: {EmpresaId}] - Empresa localizada e salva no cache.",
            request.BoatId
        );

        return new QueryResponse<BoatResponse>(viewModel);
    }
}
