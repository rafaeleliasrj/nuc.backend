using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.DomainObjects;
using NautiHub.Core.Extensions;
using NautiHub.Core.Messages.Features;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Domain.Services.InfrastructureService.Cache;
using NautiHub.Domain.Services.InfrastructureService.File;
using NautiHub.Domain.Services.InfrastructureService.File.Models.Requests;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Application.UseCases.Features.InserirLogo;

public class BoatImageAddFeatureHandler(
    DatabaseContext context,
    ILogger<BoatImageAddFeatureHandler> logger,
    IBoatRepository boatRepository,
    IFileService fileService,
    ICacheService cacheService,
    INautiHubIdentity nautiHubiIdentity
) : FeatureHandler(context), IRequestHandler<BoatImageAddFeature, FeatureResponse<BoatImageResponse>>
{
    private readonly ILogger<BoatImageAddFeatureHandler> _logger = logger;
    private readonly IBoatRepository _boatRepository = boatRepository;
    private readonly IFileService _fileService = fileService;
    private readonly ICacheService _cacheService = cacheService;
    private readonly INautiHubIdentity _nautiHubIdentity = nautiHubiIdentity;

    public async Task<FeatureResponse<BoatImageResponse>> Handle(
        BoatImageAddFeature request,
        CancellationToken cancellationToken
    )
    {
        Boat? boat = await _boatRepository.GetByIdAsync(request.BoatId);

        if (boat == null)
            return new FeatureResponse<BoatImageResponse>(ValidationResult);

        foreach (var image in request.Images)
        {
            var imageByte = image.ResizeBase64Image();
            var lastUpdate = DateTime.Now.Ticks;
            var file = new FileRequest($"{boat.Id}-{lastUpdate}.jpg", imageByte);
            var imageUrl = await _fileService.SaveAsync(file);

            //TODO: Implementar persistência de imagem
            //boat.AddImage(new BoatImage(boat.Id, imageUrl));

            await _boatRepository.UpdateAsync(boat);
        }

        await SaveChanges();

        var cacheKey = $"boat:{_nautiHubIdentity.UserId}:{request.BoatId}";
        await _cacheService.DeleteAsync(cacheKey);

        return new FeatureResponse<BoatImageResponse>(ValidationResult, new BoatImageResponse()
        {
            Images = boat.Images.ToList(),
        });
    }
}
