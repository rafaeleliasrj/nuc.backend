using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Features.PaymentCreate;
using NautiHub.Core.Resources;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para o comando de criação de pagamento
/// </summary>
public class CreatePaymentValidator : AbstractValidator<CreatePaymentFeature>
{
    public CreatePaymentValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.Data.BookingId)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Booking_Id_Required);

        RuleFor(x => x.Data.AsaasCustomerId)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Customer_Id_Required)
            .MaximumLength(100)
            .WithMessage(messagesService.Validation_Payment_Customer_Id_Too_Long);

        RuleFor(x => x.Data.Value)
            .GreaterThan(0)
            .WithMessage(messagesService.Validation_Payment_Value_Greater_Zero)
            .LessThanOrEqualTo(100000)
            .WithMessage(messagesService.Validation_Payment_Value_Max_100k);

        RuleFor(x => x.Data.Method)
            .IsInEnum()
            .WithMessage(messagesService.Validation_Payment_Method_Invalid);

        RuleFor(x => x.Data.Description)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Description_Required)
            .MaximumLength(500)
            .WithMessage(messagesService.Validation_Payment_Description_Too_Long);

        RuleFor(x => x.Data.DueDate)
            .GreaterThan(DateTime.UtcNow.AddDays(1))
            .When(x => x.Data.DueDate.HasValue)
            .WithMessage(messagesService.Validation_Payment_Due_Date_Future);

        RuleFor(x => x.Data.ExternalReference)
            .MaximumLength(255)
            .WithMessage(messagesService.Validation_Payment_External_Ref_Too_Long)
            .When(x => !string.IsNullOrEmpty(x.Data.ExternalReference));

        // Validação condicional para cartão de crédito
        When(x => x.Data.Method == PaymentMethod.CreditCard, () =>
        {
            RuleFor(x => x.Data.CreditCard)
                .NotNull()
                .WithMessage(messagesService.Validation_Payment_Card_Info_Required);

            RuleFor(x => x.Data.CreditCard!.HolderName)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Card_Holder_Required)
                .MaximumLength(255)
                .WithMessage(messagesService.Validation_Payment_Card_Holder_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.Number)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Card_Number_Required)
                .CreditCard()
                .WithMessage(messagesService.Validation_Payment_Card_Number_Invalid)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.ExpiryMonth)
                .InclusiveBetween(1, 12)
                .WithMessage(messagesService.Validation_Payment_Expiry_Month_Invalid)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.ExpiryYear)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage(messagesService.Validation_Payment_Expiry_Year_Past)
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 10)
                .WithMessage(messagesService.Validation_Payment_Expiry_Year_Future)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.Cvv)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Cvv_Required)
                .Length(3, 4)
                .WithMessage(messagesService.Validation_Payment_Cvv_Invalid_Digits)
                .Matches(@"^\d{3,4}$")
                .WithMessage(messagesService.Validation_Payment_Cvv_Numbers_Only)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.CpfCnpj)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Document_Required)
                .Must(BeValidCpfCnpj)
                .WithMessage(messagesService.Validation_Payment_Document_Invalid)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.Email)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Email_Required)
                .EmailAddress()
                .WithMessage(messagesService.Validation_Payment_Email_Invalid)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.PostalCode)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Postal_Code_Required)
                .MaximumLength(20)
                .WithMessage(messagesService.Validation_Payment_Postal_Code_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.Address)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Address_Required)
                .MaximumLength(255)
                .WithMessage(messagesService.Validation_Payment_Address_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.AddressNumber)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Address_Number_Required)
                .MaximumLength(50)
                .WithMessage(messagesService.Validation_Payment_Address_Number_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.Province)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Province_Required)
                .MaximumLength(100)
                .WithMessage(messagesService.Validation_Payment_Province_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.City)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_City_Required)
                .MaximumLength(100)
                .WithMessage(messagesService.Validation_Payment_City_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.State)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_State_Required)
                .MaximumLength(50)
                .WithMessage(messagesService.Validation_Payment_State_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.Country)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Country_Required)
                .MaximumLength(50)
                .WithMessage(messagesService.Validation_Payment_Country_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.MobilePhone)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Mobile_Required)
                .MaximumLength(20)
                .WithMessage(messagesService.Validation_Payment_Mobile_Too_Long)
                .When(x => x.Data.CreditCard != null);

            RuleFor(x => x.Data.CreditCard!.RemoteIp)
                .NotEmpty()
                .WithMessage(messagesService.Validation_Payment_Remote_Ip_Required)
                .Must(BeValidIpAddress)
                .WithMessage(messagesService.Validation_Payment_Remote_Ip_Invalid)
                .When(x => x.Data.CreditCard != null);
        });

        // Validação para splits
        RuleFor(x => x.Data.Splits)
            .Must(HaveValidSplits)
            .WithMessage(messagesService.Validation_Payment_Splits_Either_Fixed_Percentual)
            .When(x => x.Data.Splits != null && x.Data.Splits.Any());

        RuleForEach(x => x.Data.Splits)
            .SetValidator(new PaymentSplitRequestValidator(serviceProvider))
            .When(x => x.Data.Splits != null && x.Data.Splits.Any());
    }

    private static bool BeValidCpfCnpj(string cpfCnpj)
    {
        if (string.IsNullOrWhiteSpace(cpfCnpj))
            return false;

        var cleanValue = cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim();

        // CPF: 11 dígitos
        if (cleanValue.Length == 11)
        {
            return cleanValue.All(char.IsDigit) && !IsRepeatedSequence(cleanValue);
        }

        // CNPJ: 14 dígitos
        if (cleanValue.Length == 14)
        {
            return cleanValue.All(char.IsDigit) && !IsRepeatedSequence(cleanValue);
        }

        return false;
    }

    private static bool IsRepeatedSequence(string value)
    {
        return value.Distinct().Count() == 1;
    }

    private static bool BeValidIpAddress(string ip)
    {
        return System.Net.IPAddress.TryParse(ip, out _);
    }

    private static bool HaveValidSplits(List<PaymentSplitRequest> splits)
    {
        if (splits == null || !splits.Any())
            return true;

        return splits.All(split => 
            (split.FixedValue.HasValue && split.FixedValue.Value > 0 && !split.PercentualValue.HasValue) ||
            (split.PercentualValue.HasValue && split.PercentualValue.Value > 0 && split.PercentualValue.Value <= 100 && !split.FixedValue.HasValue));
    }
}
