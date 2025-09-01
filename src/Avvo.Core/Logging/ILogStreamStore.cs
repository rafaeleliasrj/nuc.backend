namespace Avvo.Core.Logging
{
    /// <summary>
    /// This interface defines a logstream store
    /// </summary>
    public interface ILogStreamStore
    {
        /// <summary>
        /// This method is called to add a log to the store
        /// </summary>
        /// <param name="entry">The log entry to add</param>
        void Add(LogEntry entry);

        /// <summary>
        /// This method is called to get a log entry from the store
        /// </summary>
        LogEntry Get();

        /// <summary>
        /// This method is called to remove an entry from the store
        /// </summary>
        /// <param name="entry">The log entry to remove</param>
        void Remove(LogEntry entry);
    }
}
