using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

public class BoatListFilterRequestValidator
    : AbstractValidator<BoatListFilterRequest>
{
    public BoatListFilterRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        
        RuleFor(r => r.Page).NotNull().WithMessage(messagesService.Validation_Page_Required);

        RuleFor(r => r.PerPage)
            .NotNull()
            .WithMessage(messagesService.Validation_PageSize_Required);

        RuleFor(r => r.Page).Must(r => r > 0).WithMessage(messagesService.Validation_Page_Greater_Zero);

        RuleFor(r => r.PerPage)
            .Must(r => r > 0)
            .WithMessage(messagesService.Validation_PageSize_Greater_Zero);

        RuleFor(r => r.PerPage)
            .Must(r => r <= 100)
            .WithMessage(messagesService.Validation_PageSize_Max_100);
    }
}