using FluentValidation.Results;

namespace NautiHub.Core.Messages.Commands;

public interface ICommandResponse
{
    public void SetValidationResult(ValidationResult validationResult);
}

public class CommandResponse : ICommandResponse
{
    public ValidationResult ValidationResult { get; private set; }

    public CommandResponse(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }

    public CommandResponse()
    {
        ValidationResult = new();
    }

    public void SetValidationResult(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }
}

public class CommandResponse<TResponse> : ICommandResponse
{
    public ValidationResult ValidationResult { get; private set; }

    public TResponse Response { get; private set; }

    public CommandResponse(ValidationResult validationResult, TResponse response = default!)
    {
        ValidationResult = validationResult;
        Response = response;
    }

    public CommandResponse(TResponse response)
    {
        ValidationResult = new();
        Response = response;
    }

    public CommandResponse()
    {
        ValidationResult = new();
        Response = default!;
    }

    public void SetValidationResult(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }
}
