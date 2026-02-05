using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Core.DomainObjects;
using NautiHub.Core.Messages.Features;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Domain.Services.InfrastructureService.Cache;
using NautiHub.Infrastructure.DataContext;
using System.Net;

namespace NautiHub.Application.UseCases.Features.AtualizarEmpresa;

public class BoatUpdateFeatureHandler(
    DatabaseContext context,
    ILogger<BoatUpdateFeatureHandler> logger,
    IBoatRepository boatRepository,
    ICacheService cacheService,
    INautiHubIdentity nautiHubIdentity
    )
        : FeatureHandler(context),
        IRequestHandler<BoatUpdateFeature, FeatureResponse<bool>>
{
    private readonly ILogger<BoatUpdateFeatureHandler> _logger = logger;
    private readonly IBoatRepository _boatRepository = boatRepository;
    private readonly ICacheService _cacheService = cacheService;
    private readonly INautiHubIdentity _nautiHubIdentity = nautiHubIdentity;

    public async Task<FeatureResponse<bool>> Handle(
        BoatUpdateFeature request,
        CancellationToken cancellationToken
    )
    {
        await Validate(request);

        if (HasError())
            return new FeatureResponse<bool>(ValidationResult, false, HttpStatusCode.BadRequest);

        Boat? boat = await _boatRepository.GetByIdAsync(request.BoatId);

        if (boat == null)
            return new FeatureResponse<bool>(ValidationResult, false);

        boat.UpdateBasicInfo(
            request.Data.Name,
            request.Data.Description,
            request.Data.Capacity,
            request.Data.PricePerPerson,
            request.Data.LocationCity,
            request.Data.LocationState,
            request.Data.BoatType
        );

        boat.UpdateAmenities(request.Data.Amenities);


        //TODO: Implementar persistência do documento
        boat.UpdateDocument(
            request.Data.Document,
            request.Data.DocumentUrl);

        await _boatRepository.UpdateAsync(boat);
        await SaveChanges();

        var cacheKey = $"boat:{_nautiHubIdentity.UserId}:{request.BoatId}";
        await _cacheService.DeleteAsync(cacheKey);

        boat = await _boatRepository.GetByIdAsync(request.BoatId);

        return new FeatureResponse<bool>(ValidationResult, true);
    }

    private async Task<FeatureResponse<Guid>?> Validate(BoatUpdateFeature request)
    {
        return new FeatureResponse<Guid>(ValidationResult);
    }
}
