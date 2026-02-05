using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ExcluirLogo;

public class BoatImageDeleteFeature : Feature<FeatureResponse<bool>>
{
    public Guid BoatId { get; set; }
}
