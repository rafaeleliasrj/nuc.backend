namespace Avvo.Core.Logging
{
    using System.Text;
    using System;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Avvo.Core.Logging.Correlation;
    using System.Collections.Generic;

    /// <summary>
    /// This is the custom Logger.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly string categoryName;
        private readonly ICorrelationService correlationService;
        private readonly IApplicationDetails applicationDetails;
        private LogFormat? logFormat;
        private Formatting? jsonFormatting = null;

        /// <summary>
        /// This is the ILogStreamStore to use
        /// </summary>
        public ILogStreamStore LogStreamStore { get; set; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="categoryName">The name of the log category</param>
        /// <param name="correlationService">The ICorrelationService to use</param>
        /// <param name="applicationDetails">The IApplicationDetails to use</param>
        /// <param name="logFormat">The LogFormat to use</param>
        public Logger(
            string categoryName,
            ICorrelationService correlationService,
            IApplicationDetails applicationDetails,
            LogFormat? logFormat = null)
        {
            this.categoryName = categoryName;
            this.correlationService = correlationService;
            this.logFormat = logFormat;
            this.applicationDetails = applicationDetails;
        }

        /// <summary>
        /// This property is used to get/set the format of the log entries.
        /// If not specified in code it will attempt to read the value from the `LOG_FORMAT` environment variable.
        /// TEXT is the default value when no format is specified.
        /// </summary>
        public LogFormat Format
        {
            get
            {
                if (this.logFormat == null)
                {
                    string logFormat = Environment.GetEnvironmentVariable("LOG_FORMAT");
                    if (string.IsNullOrEmpty(logFormat))
                    {
                        logFormat = "text";
                    }

                    switch (logFormat.ToLower())
                    {
                        case "json":
                            this.logFormat = LogFormat.JSON;
                            break;
                        default:
                            this.logFormat = LogFormat.TEXT;
                            break;
                    }
                }
                return this.logFormat.Value;
            }
            set { this.logFormat = value; }
        }

        /// <summary>
        /// This method is called to create and format the log entry.
        /// </summary>
        /// <param name="logLevel">This is the level of the log message. (Debug,Warning,Info etc.)</param>
        /// <param name="eventId">This is a function level tracking id for grouping logging messages together.</param>
        /// <param name="state">This is the message to log.</param>
        /// <param name="exception">This is the expection to log.</param>
        /// <param name="formatter">This is the text log formatter.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            LogEntry entry = new LogEntry()
            {
                LogLevel = logLevel,
                LogName = this.categoryName,
                Message = state,
                Exception = exception,
                Correlation = new CorrelationData()
                {
                    Request = this.correlationService.Request,
                    Session = this.correlationService.Session,
                    Trace = this.correlationService.Trace,
                    Span = SpanData.Current
                },
                EventId = eventId.Id,
                Application = this.applicationDetails,
                Timestamp = DateTime.UtcNow
            };

            this.SendToStream(entry);

            switch (Format)
            {
                case LogFormat.JSON:
                    LogJson<TState>(entry);
                    break;
                default:
                case LogFormat.TEXT:
                    LogText<TState>(entry, formatter);
                    break;
            }
        }

        /// <summary>
        /// This method is called to output the log entry as json.
        /// </summary>
        /// <param name="entry">This is the log entry item to output.</param>
        private void LogJson<TState>(LogEntry entry)
        {
            var formatting = GetJsonFormatting();
            string message_json = JsonConvert.SerializeObject(new LogOutput(entry), formatting);
            Console.WriteLine(message_json);
        }

        private Formatting GetJsonFormatting()
        {
            if (jsonFormatting is null)
            {
                var environmentsLogs = new List<string>() { "LOCAL", "TEST", "DOCKER" };
                var environment = Environment.GetEnvironmentVariable("ENVIRONMENT")?.ToUpperInvariant();
                if (environmentsLogs.Contains(environment))
                    jsonFormatting = Formatting.Indented;
                else
                    jsonFormatting = Formatting.None;
            }

            return jsonFormatting.Value;
        }

        /// <summary>
        /// This method is called to output the log entry as text.
        /// </summary>
        /// <param name="entry">This is the log entry item to output.</param>
        /// <param name="formatter">This is the text formatter to use.</param>
        private void LogText<TState>(LogEntry entry, Func<TState, Exception, string> formatter)
        {
            StringBuilder message_text = new StringBuilder();
            message_text.Append($"{entry.LogLevel}: ");
            message_text.Append($"[Trace: {entry.Correlation.Trace}] - [Span: {entry.Correlation.Span.Id}] - [Request: {entry.Correlation.Request.Id}] - [Session: {entry.Correlation.Session}] - ");
            message_text.Append($"[{entry.LogName}] : {entry.Message}");

            if (entry.Correlation.Span.Duration != null)
            {
                message_text.Append($" - [Duration: {String.Format("{0:0.000}", entry.Correlation.Span.Duration)} seconds]");
            }

            if (entry.Exception != null)
            {
                message_text.Append($" - [Exception] {entry.Exception}");
            }

            Console.WriteLine(message_text.ToString());
        }

        /// <summary>
        /// This method is called to determine if this logger is enabled.
        /// </summary>
        /// <param name="logLevel">The current LogLevel</param>
        public bool IsEnabled(LogLevel logLevel)
        {
            string level = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "DEBUG";
            if (!Enum.TryParse<LogLevel>(level, true, out var enumLevel))
                enumLevel = LogLevel.Debug;

            return logLevel >= (LogLevel)enumLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// This method is called to send a log entry to the log stream
        /// </summary>
        /// <param name="entry">The current LogEntry to send</param>
        private void SendToStream(LogEntry entry)
        {
            if (this.LogStreamStore == null)
            {
                return;
            }

            this.LogStreamStore.Add(entry);
        }
    }
}
