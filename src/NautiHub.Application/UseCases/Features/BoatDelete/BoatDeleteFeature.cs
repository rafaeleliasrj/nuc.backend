using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ExcluirEmpresa;

public class BoatDeleteFeature : Feature<FeatureResponse<bool>>
{
    public Guid BoatId { get; set; }
}
