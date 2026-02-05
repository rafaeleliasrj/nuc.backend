using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para RefundPaymentRequest
/// </summary>
public class RefundPaymentRequestValidator : AbstractValidator<RefundPaymentRequest>
{
    public RefundPaymentRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.Value)
            .GreaterThan(0)
            .When(x => x.Value.HasValue)
            .WithMessage(messagesService.Validation_Refund_Value_Greater_Zero);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Refund_Reason_Required)
            .MaximumLength(500)
            .WithMessage(messagesService.Validation_Refund_Reason_Too_Long);
    }
}