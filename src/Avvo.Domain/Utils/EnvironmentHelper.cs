using Avvo.Core.Commons.Utils;

namespace Avvo.Domain.Utils;

public class EnvironmentHelper
{
    private static IList<string> LOCAL_ENVIRONMENTS = new List<string>() { "local", "test", "docker" };
    public static bool IsLocalExecution() => LOCAL_ENVIRONMENTS.Contains((Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "local").ToLowerInvariant());
    public static Microsoft.Extensions.Logging.LogLevel GetLogLevel()
    {
        if (Enum.TryParse<Microsoft.Extensions.Logging.LogLevel>(EnvironmentVariables.Get("LOG_LEVEL"), true, out Microsoft.Extensions.Logging.LogLevel logLevel))
            return logLevel;
        else return Microsoft.Extensions.Logging.LogLevel.Trace;
    }
}
