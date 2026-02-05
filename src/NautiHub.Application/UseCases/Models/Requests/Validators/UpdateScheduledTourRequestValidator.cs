using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para UpdateScheduledTourRequest
/// </summary>
public class UpdateScheduledTourRequestValidator : AbstractValidator<UpdateScheduledTourRequest>
{
    public UpdateScheduledTourRequestValidator(IServiceProvider serviceProvider)
    {
        RuleFor(x => x.TourDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.TourDate.HasValue)
            .WithMessage("Tour date cannot be in the past");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .When(x => x.StartTime.HasValue)
            .WithMessage("Start time is required");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .When(x => x.EndTime.HasValue)
            .WithMessage("End time is required")
            .GreaterThan(x => x.StartTime)
            .When(x => x.StartTime.HasValue && x.EndTime.HasValue)
            .WithMessage("End time must be after start time");

        RuleFor(x => x.AvailableSeats)
            .GreaterThan(0)
            .When(x => x.AvailableSeats.HasValue)
            .WithMessage("Available seats must be greater than 0")
            .LessThanOrEqualTo(1000)
            .When(x => x.AvailableSeats.HasValue)
            .WithMessage("Available seats cannot exceed 1000");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 1000 characters");
    }
}