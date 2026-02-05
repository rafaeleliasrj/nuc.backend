namespace NautiHub.Core.Extensions;

public static class ExceptionExtensions
{
    public static bool MaximumAttemptReached(this Exception ex)
    {
        if (string.IsNullOrWhiteSpace(ex.Message))
            return false;

        return ex.Message.Contains("Número máximo de tentativas atingido");
    }
}
