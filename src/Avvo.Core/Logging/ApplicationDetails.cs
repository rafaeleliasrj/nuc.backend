namespace Avvo.Core.Logging
{
    using System;

    /// <summary>
    /// This class is used to define application details.
    /// </summary>
    public class ApplicationDetails : IApplicationDetails
    {
        private string name;

        /// <summary>
        /// This is the name of this application.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    this.name = System.Environment.GetEnvironmentVariable("APPLICATION_NAME");
                }
                return this.name;
            }
        }

        private string version;

        /// <summary>
        /// This is the version of this application.
        /// </summary>
        public string Version
        {
            get
            {
                if (this.version == null)
                {
                    this.version = System.Environment.GetEnvironmentVariable("VERSION");
                }
                return this.version;
            }
        }

        private string configVersion;

        /// <summary>
        /// This is the version of the config applied to this application.
        /// </summary>
        public string ConfigVersion
        {
            get
            {
                if (this.configVersion == null)
                {
                    this.configVersion = System.Environment.GetEnvironmentVariable("CONFIG_VERSION") ?? this.Version;
                }
                return this.configVersion;
            }
        }

        private string landscape;

        /// <summary>
        /// This is the landscape/region this application is running in.
        /// </summary>
        public string Landscape
        {
            get
            {
                if (this.landscape == null)
                {
                    this.landscape = System.Environment.GetEnvironmentVariable("LANDSCAPE");
                }
                return this.landscape;
            }
        }

        private string environment;

        /// <summary>
        /// This is the environment this application is running in.
        /// </summary>
        public string Environment
        {
            get
            {
                if (this.environment == null)
                {
                    this.environment = System.Environment.GetEnvironmentVariable("ENVIRONMENT");
                }
                return this.environment;
            }
        }

        private string configEnvironment;

        /// <summary>
        /// This is the environment config applied to this application.
        /// </summary>
        public string ConfigEnvironment
        {
            get
            {
                if (this.configEnvironment == null)
                {
                    this.configEnvironment = System.Environment.GetEnvironmentVariable("CONFIG_ENVIRONMENT") ?? this.Environment;
                }
                return this.configEnvironment;
            }
        }
    }
}
