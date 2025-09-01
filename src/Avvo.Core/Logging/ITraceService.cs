namespace Avvo.Core.Logging
{
    /// <summary>
    /// This interface is used to define a correlation trace service.
    /// </summary>
    public interface ITraceService
    {
        /// <summary>
        /// This method is called to start a correlation trace.
        /// </summary>
        /// <param name="message">The trace message.</param>
        /// <param name="name">The name of the trace.</param>
        /// <param name="category">The category of the trace.</param>
        /// <param name="subCategory">The sub-category of the trace.</param>
        /// <param name="debug">Should this trace be a debug log.</param>
        /// <returns>ITracer</returns>
        ITracer Trace(string message, string name = null, string category = null, string subCategory = null, bool debug = false);
    }
}
