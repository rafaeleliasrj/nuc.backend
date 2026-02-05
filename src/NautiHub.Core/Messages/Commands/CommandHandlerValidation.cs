using FluentValidation;
using FluentValidation.Results;

using MediatR;

namespace NautiHub.Core.Messages.Commands;

public class CommandHandlerValidation<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ICommandResponse
{
    private readonly IEnumerable<IValidator> _validators = validators;

    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        var possuiErros = failures.Any();

        if (possuiErros)
        {
            return Errors(failures);
        }

        return next();
    }

    private static async Task<TResponse> Errors(IEnumerable<ValidationFailure> failures)
    {
        var validationResult = new ValidationResult() { Errors = failures?.ToList() };
        TResponse response = Activator.CreateInstance<TResponse>();
        response.SetValidationResult(validationResult);
        return (await Task.Run(() => response))!;
    }
}
