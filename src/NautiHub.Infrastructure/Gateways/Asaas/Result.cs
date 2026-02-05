using System.Collections.Generic;

namespace NautiHub.Infrastructure.Gateways.Asaas;

/// <summary>
/// Result genérico para operações do Asaas
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; }
    public T Data { get; private set; }

    private Result(bool isSuccess, string error, T data = default)
    {
        IsSuccess = isSuccess;
        Error = error;
        Data = data;
    }

    public static Result<T> Success(T data)
    {
        return new Result<T>(true, string.Empty, data);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, error, default);
    }
}