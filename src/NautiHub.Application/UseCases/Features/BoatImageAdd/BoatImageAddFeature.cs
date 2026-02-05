using FluentValidation;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.InserirLogo;

public class BoatImageAddFeature : Feature<FeatureResponse<BoatImageResponse>>
{
    public Guid BoatId { get; set; }

    public IList<string> Images { get; set; }
}

public class BoatImageAddFeatureFeatureValidator : AbstractValidator<BoatImageAddFeature>
{
    public BoatImageAddFeatureFeatureValidator()
    {
        RuleFor(r => r.Images)
        .NotNull()
        .NotEmpty()
        .WithMessage("É obrigatório informar ao menos uma imagem.");

        RuleForEach(r => r.Images)
            .NotEmpty()
            .WithMessage("A imagem não pode estar vazia.")
            .Must(image => image.StartsWith("data:image/"))
            .WithMessage("Imagem não é válida. O formato esperado é data:image/*.");
    }
}
