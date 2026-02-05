using FluentValidation.Results;

using System.Net;

namespace NautiHub.Core.Messages.Features;

public interface IFeatureResponse
{
    public void SetValidationResult(ValidationResult validationResult);
    public HttpStatusCode StatusCode { get; set; }
}

public class FeatureResponse : IFeatureResponse
{
    public ValidationResult ValidationResult { get; private set; }
    public HttpStatusCode StatusCode { get; set; }


    public FeatureResponse()
    {
        ValidationResult = new();
        StatusCode = HttpStatusCode.OK;
    }

    public FeatureResponse(ValidationResult validationResult, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        ValidationResult = validationResult;
        StatusCode = statusCode;
    }

    public FeatureResponse(HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        ValidationResult = new();
        StatusCode = statusCode;
    }

    public void SetValidationResult(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }
}

public class FeatureResponse<TResponse> : IFeatureResponse
{
    public ValidationResult ValidationResult { get; private set; }
    public TResponse Response { get; private set; }
    public HttpStatusCode StatusCode { get; set; }


    public FeatureResponse()
    {
        ValidationResult = new();
        Response = default!;
        StatusCode = HttpStatusCode.OK;
    }

    public FeatureResponse(ValidationResult validationResult, TResponse response = default!, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        ValidationResult = validationResult;
        Response = response;
        StatusCode = statusCode;
    }

    public FeatureResponse(TResponse response, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        ValidationResult = new();
        Response = response;
        StatusCode = statusCode;
    }

    public FeatureResponse(HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        ValidationResult = new();
        Response = default!;
        StatusCode = statusCode;
    }

    public void SetValidationResult(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }

    public static implicit operator FeatureResponse<TResponse>(FeatureResponse<bool> v) => throw new NotImplementedException();
}
