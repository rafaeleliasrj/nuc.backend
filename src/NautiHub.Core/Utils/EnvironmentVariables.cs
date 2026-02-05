namespace NautiHub.Core.Utils;

public class EnvironmentVariables
{
    /// <summary>IDependencyGroupIDependencyGroup
    /// This method is called to set an environment variable for this process.
    /// </summary>
    /// <param name="name">The unique name of the environment variable</param>
    /// <param name="value">The value of the environment variable</param>
    public static void Set(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);
    }

    /// <summary>
    /// This method is called to set an environment variable for this process only if it is not already set.
    /// </summary>
    /// <param name="name">The unique name of the environment variable</param>
    /// <param name="value">The value of the environment variable</param>
    public static void SetIfNull(string name, string value)
    {
        if (Get(name) != null)
        {
            return;
        }
        Set(name, value);
    }

    /// <summary>
    /// This method is called to get an environment variable for this process.
    /// </summary>
    /// <param name="name">The unique name of the environment variable</param>
    /// <returns>value of the environment variable</returns>
    public static string Get(string name)
    {
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process)!;
    }

    /// <summary>
    /// This method is called to delete an environment variable for this process.
    /// </summary>
    /// <param name="name">The unique name of the environment variable</param>
    public static void Delete(string name)
    {
        Environment.SetEnvironmentVariable(name, null, EnvironmentVariableTarget.Process);
    }
}
