using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para o request de split de pagamento
/// </summary>
public class PaymentSplitRequestValidator : AbstractValidator<PaymentSplitRequest>
{
    public PaymentSplitRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.WalletId)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Split_Wallet_Required)
            .MaximumLength(100)
            .WithMessage(messagesService.Validation_Payment_Split_Wallet_Too_Long);

        RuleFor(x => x.FixedValue)
            .GreaterThan(0)
            .WithMessage(messagesService.Validation_Payment_Split_Fixed_Greater_Zero)
            .When(x => x.FixedValue.HasValue);

        RuleFor(x => x.PercentualValue)
            .GreaterThan(0)
            .WithMessage(messagesService.Validation_Payment_Split_Percentual_Greater_Zero)
            .LessThanOrEqualTo(100)
            .WithMessage(messagesService.Validation_Payment_Split_Percentual_Max_100)
            .When(x => x.PercentualValue.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage(messagesService.Validation_Payment_Split_Description_Too_Long)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x)
            .Must(HaveValidSplitType)
            .WithMessage(messagesService.PaymentSplit_Either_Fixed_Percentual);
    }

    private static bool HaveValidSplitType(PaymentSplitRequest split)
    {
        var hasFixedValue = split.FixedValue.HasValue && split.FixedValue.Value > 0;
        var hasPercentualValue = split.PercentualValue.HasValue && split.PercentualValue.Value > 0;

        return hasFixedValue ^ hasPercentualValue; // XOR - um ou outro, mas não ambos
    }
}