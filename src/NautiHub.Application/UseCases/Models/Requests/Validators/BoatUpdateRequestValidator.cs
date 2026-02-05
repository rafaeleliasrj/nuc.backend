using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

public class BoatUpdateRequestValidator : AbstractValidator<BoatUpdateRequest>
{
    public BoatUpdateRequestValidator(IServiceProvider serviceProvider)
    {
        
    }
}