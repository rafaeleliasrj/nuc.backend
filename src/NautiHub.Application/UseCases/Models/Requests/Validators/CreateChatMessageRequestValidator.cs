using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para CreateChatMessageRequest
/// </summary>
public class CreateChatMessageRequestValidator : AbstractValidator<CreateChatMessageRequest>
{
    public CreateChatMessageRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage(messagesService.Validation_Booking_Id_Required);

        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage(messagesService.Validation_Customer_Id_Required);

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage(messagesService.Validation_Comment_Too_Long)
            .MaximumLength(1000).WithMessage(messagesService.Validation_Notes_Too_Long);
    }
}