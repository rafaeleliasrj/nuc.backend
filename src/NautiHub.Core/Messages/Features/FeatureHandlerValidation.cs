using FluentValidation;
using FluentValidation.Results;

using MediatR;

namespace NautiHub.Core.Messages.Features;

public class FeatureHandlerValidation<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IFeatureResponse
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
            return await Errors(failures);

        return await next();
    }

    private static Task<TResponse> Errors(IEnumerable<ValidationFailure> failures)
    {
        var validationResult = new ValidationResult(failures.ToList());
        TResponse response = Activator.CreateInstance<TResponse>()!;
        response.SetValidationResult(validationResult);
        return Task.FromResult(response);
    }
}
