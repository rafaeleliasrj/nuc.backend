namespace Avvo.Core.Logging
{
    /// <summary>
    /// This interface is used to define a Log stream.
    /// </summary>
    public interface ILogStream
    {
        /// <summary>
        /// This method is called to start this log stream.
        /// </summary>
        void Start();
    }
}
