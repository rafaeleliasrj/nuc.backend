using FluentValidation.Results;

namespace NautiHub.Core.Messages.Queries;

public interface IQueryResponse
{
    public void SetValidationResult(ValidationResult validationResult);
}

public class QueryResponse<TResponse> : IQueryResponse
{
    public ValidationResult ValidationResult { get; private set; }

    public TResponse Response { get; private set; }

    public QueryResponse(ValidationResult validationResult, TResponse response = default!)
    {
        ValidationResult = validationResult;
        Response = response;
    }

    public QueryResponse(TResponse response)
    {
        ValidationResult = new();
        Response = response;
    }

    public QueryResponse()
    {
        ValidationResult = new();
        Response = default!;
    }

    public void SetValidationResult(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }
}
