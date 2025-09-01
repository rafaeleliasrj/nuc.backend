namespace Avvo.Core.Logging
{
    /// <summary>
    /// This interface defines the details of an application
    /// </summary>
    public interface IApplicationDetails
    {
        /// <summary>
        /// This is the name of this application
        /// </summary>
        string Name { get; }

        /// <summary>
        /// This is the version of this application
        /// </summary>
        string Version { get; }

        /// <summary>
        /// This is the config version of this application
        /// </summary>
        string ConfigVersion { get; }

        /// <summary>
        /// This is the landscape this application is running in
        /// </summary>
        string Landscape { get; }

        /// <summary>
        /// This is the environment this application is running in
        /// </summary>
        string Environment { get; }

        /// <summary>
        /// This is the config environment this application is using
        /// </summary>
        string ConfigEnvironment { get; }
    }
}
