using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

public class BoatRequestValidator : AbstractValidator<BoatRequest>
{
    /// <summary/>
    public BoatRequestValidator(IServiceProvider serviceProvider)
    {

    }
}