using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para UpdateScheduledTourStatusRequest
/// </summary>
public class UpdateScheduledTourStatusRequestValidator : AbstractValidator<UpdateScheduledTourStatusRequest>
{
    public UpdateScheduledTourStatusRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(messagesService.Validation_Invalid_Tour_Status);

        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Reason))
            .WithMessage(messagesService.Validation_Reason_Too_Long);
    }
}