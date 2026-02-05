using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para UpdatePaymentRequest
/// </summary>
public class UpdatePaymentRequestValidator : AbstractValidator<UpdatePaymentRequest>
{
    public UpdatePaymentRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage(messagesService.Validation_Payment_Description_Too_Long);

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.Now)
            .When(x => x.DueDate.HasValue)
            .WithMessage(messagesService.Validation_Payment_Due_Date_Future);

        RuleFor(x => x.ExternalReference)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.ExternalReference))
            .WithMessage(messagesService.Validation_Payment_External_Ref_Too_Long);
    }
}