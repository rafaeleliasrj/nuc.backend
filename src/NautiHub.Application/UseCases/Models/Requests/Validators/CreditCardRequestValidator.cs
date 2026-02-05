using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para CreditCardRequest
/// </summary>
public class CreditCardRequestValidator : AbstractValidator<CreditCardRequest>
{
    public CreditCardRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.HolderName)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Card_Holder_Required)
            .MaximumLength(100)
            .WithMessage(messagesService.Validation_Payment_Card_Holder_Too_Long);

        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Card_Number_Required)
            .CreditCard()
            .WithMessage(messagesService.Validation_Payment_Card_Number_Invalid);

        RuleFor(x => x.ExpiryMonth)
            .InclusiveBetween(1, 12)
            .WithMessage(messagesService.Validation_Payment_Expiry_Month_Invalid);

        RuleFor(x => x.ExpiryYear)
            .GreaterThanOrEqualTo(DateTime.Now.Year)
            .WithMessage(messagesService.Validation_Payment_Expiry_Year_Past);

        RuleFor(x => x.Cvv)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Cvv_Required)
            .Length(3, 4)
            .WithMessage(messagesService.Validation_Payment_Cvv_Invalid_Digits)
            .Matches(@"^\d+$")
            .WithMessage(messagesService.Validation_Payment_Cvv_Numbers_Only);

        RuleFor(x => x.CpfCnpj)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Document_Required)
            .Must(BeValidCpfCnpj)
            .WithMessage(messagesService.Validation_Payment_Document_Invalid);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Email_Required)
            .EmailAddress()
            .WithMessage(messagesService.Validation_Payment_Email_Invalid);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Postal_Code_Required)
            .MaximumLength(20)
            .WithMessage(messagesService.Validation_Payment_Postal_Code_Too_Long);

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Address_Required)
            .MaximumLength(200)
            .WithMessage(messagesService.Validation_Payment_Address_Too_Long);

        RuleFor(x => x.AddressNumber)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Address_Number_Required)
            .MaximumLength(20)
            .WithMessage(messagesService.Validation_Payment_Address_Number_Too_Long);

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_City_Required)
            .MaximumLength(100)
            .WithMessage(messagesService.Validation_Payment_City_Too_Long);

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_State_Required)
            .MaximumLength(50)
            .WithMessage(messagesService.Validation_Payment_State_Too_Long);

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Country_Required)
            .MaximumLength(50)
            .WithMessage(messagesService.Validation_Payment_Country_Too_Long);

        RuleFor(x => x.MobilePhone)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Mobile_Required)
            .MaximumLength(20)
            .WithMessage(messagesService.Validation_Payment_Mobile_Too_Long);

        RuleFor(x => x.RemoteIp)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Payment_Remote_Ip_Required)
            .Matches(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$")
            .WithMessage(messagesService.Validation_Payment_Remote_Ip_Invalid);
    }

    private static bool BeValidCpfCnpj(string cpfCnpj)
    {
        if (string.IsNullOrWhiteSpace(cpfCnpj))
            return false;

        // Remove non-numeric characters
        cpfCnpj = System.Text.RegularExpressions.Regex.Replace(cpfCnpj, @"[^\d]", "");

        // CPF validation
        if (cpfCnpj.Length == 11)
        {
            return IsValidCpf(cpfCnpj);
        }
        // CNPJ validation
        else if (cpfCnpj.Length == 14)
        {
            return IsValidCnpj(cpfCnpj);
        }

        return false;
    }

    private static bool IsValidCpf(string cpf)
    {
        // Basic CPF validation logic (simplified)
        return cpf.Length == 11 && cpf.Distinct().Count() >= 2;
    }

    private static bool IsValidCnpj(string cnpj)
    {
        // Basic CNPJ validation logic (simplified)
        return cnpj.Length == 14 && cnpj.Distinct().Count() >= 2;
    }
}