using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;

namespace NautiHub.Application.UseCases.Models.Requests.Validators;

/// <summary>
/// Validador para MarkChatMessageReadRequest
/// </summary>
public class MarkChatMessageReadRequestValidator : AbstractValidator<MarkChatMessageReadRequest>
{
    public MarkChatMessageReadRequestValidator(IServiceProvider serviceProvider)
    {
        // Não há validações específicas necessárias para este request,
        // pois IsRead é um booleano que pode ser true ou false
        // Adicionando validação apenas para garantir que o objeto foi instanciado
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Mark chat message read request is required");
    }
}