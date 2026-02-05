using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para RefundRequest
/// </summary>
public class RefundRequestValidator : AbstractValidator<RefundRequest>
{
    public RefundRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage(messagesService.Validation_Refund_Value_Greater_Zero);

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Refund_Reason_Required)
            .MaximumLength(500)
            .WithMessage(messagesService.Validation_Refund_Reason_Too_Long);
    }
}