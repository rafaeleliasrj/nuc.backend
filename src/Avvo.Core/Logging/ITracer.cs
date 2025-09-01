namespace Avvo.Core.Logging
{
    using System;

    /// <summary>
    /// This interface is used to define a correlation tracer.
    /// </summary>
    public interface ITracer : IDisposable
    {
        SpanData Data { get; }
        bool Debug { get; }
        string Message { get; }
    }
}
