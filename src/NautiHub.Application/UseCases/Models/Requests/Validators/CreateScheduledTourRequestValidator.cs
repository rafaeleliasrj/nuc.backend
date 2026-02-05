using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para CreateScheduledTourRequest
/// </summary>
public class CreateScheduledTourRequestValidator : AbstractValidator<CreateScheduledTourRequest>
{
    public CreateScheduledTourRequestValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(x => x.BoatId)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Boat_Id_Required);

        RuleFor(x => x.TourDate)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Tour_Date_Past)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(messagesService.ScheduledTour_Date_Past);

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage(messagesService.Validation_Start_Time_Required);

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage(messagesService.Validation_End_Time_Required)
            .GreaterThan(x => x.StartTime)
            .WithMessage(messagesService.Validation_End_Time_After_Start);

        RuleFor(x => x.AvailableSeats)
            .GreaterThan(0)
            .WithMessage(messagesService.Validation_Available_Seats_Greater_Zero)
            .LessThanOrEqualTo(1000)
            .WithMessage(messagesService.Validation_Available_Seats_Max_1000);

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage(messagesService.Validation_Notes_Too_Long);
    }
}