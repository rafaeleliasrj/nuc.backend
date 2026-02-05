using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Requests.Validators;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Features.AtualizarEmpresa;

public class BoatUpdateFeature : Feature<FeatureResponse<bool>>
{
    public Guid BoatId { get; set; }

    public BoatUpdateRequest Data { get; set; }
}

public class BoatUpdateFeatureValidator
    : AbstractValidator<BoatUpdateFeature>
{
    public BoatUpdateFeatureValidator(IServiceProvider serviceProvider)
    {
        RuleFor(r => r.Data)
            .SetValidator(new BoatUpdateRequestValidator(serviceProvider));
    }
}
