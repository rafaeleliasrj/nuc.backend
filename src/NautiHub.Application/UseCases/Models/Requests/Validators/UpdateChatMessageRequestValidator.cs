using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para UpdateChatMessageRequest
/// </summary>
public class UpdateChatMessageRequestValidator : AbstractValidator<UpdateChatMessageRequest>
{
    public UpdateChatMessageRequestValidator(IServiceProvider serviceProvider)
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters");
    }
}