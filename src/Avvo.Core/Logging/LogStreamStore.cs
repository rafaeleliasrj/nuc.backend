namespace Avvo.Core.Logging
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class is the log store for a stream to process
    /// </summary>
    public class LogStreamStore : ILogStreamStore
    {
        static List<LogEntry> logs = new List<LogEntry>();

        /// <summary>
        /// This method is called to add a log to the store
        /// </summary>
        /// <param name="entry">The log entry to add</param>
        public void Add(LogEntry entry)
        {
            logs.Add(entry);
        }

        /// <summary>
        /// This method is called to get a log entry from the store
        /// </summary>
        public LogEntry Get()
        {
            return logs.FirstOrDefault();
        }

        /// <summary>
        /// This method is called to remove an entry from the store
        /// </summary>
        /// <param name="entry">The log entry to remove</param>
        public void Remove(LogEntry entry)
        {
            logs.Remove(entry);
        }
    }
}
