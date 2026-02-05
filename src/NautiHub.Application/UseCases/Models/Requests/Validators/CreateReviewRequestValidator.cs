using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para CreateReviewRequest
/// </summary>
public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Booking_Id_Required);

        RuleFor(x => x.BoatId)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Boat_Id_Required);

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Customer_Id_Required);

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage(messagesService.Validation_Rating_Between_1_5);

        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Comment))
            .WithMessage(messagesService.Validation_Comment_Too_Long);
    }
}