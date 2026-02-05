using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Requests.Validators;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Features.CadastrarEmpresa;

public class BoatCreateFeature : Feature<FeatureResponse<Guid>>
{
    public BoatRequest Data { get; set; }
}

public class CreateBoatFeatureValidator : AbstractValidator<BoatCreateFeature>
{
    public CreateBoatFeatureValidator(IServiceProvider serviceProvider)
    {
        RuleFor(r => r.Data).SetValidator(new BoatRequestValidator(serviceProvider));
    }
}
