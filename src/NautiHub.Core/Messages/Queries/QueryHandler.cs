namespace NautiHub.Core.Messages.Queries;

using FluentValidation.Results;

public class QueryHandler
{
    protected ValidationResult ValidationResult;
    public QueryHandler() => ValidationResult = new();
    protected void AddError(string message) => ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
    protected bool HasError() => !ValidationResult.IsValid;
}
