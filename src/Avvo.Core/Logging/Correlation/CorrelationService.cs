namespace Avvo.Core.Logging.Correlation
{
    using System.Threading;
    using System;

    /// <summary>
    /// This is the service class used to access the current correlation tracking data.
    /// </summary>
    public class CorrelationService : ICorrelationService
    {
        private static readonly AsyncLocal<Guid> trace = new AsyncLocal<Guid>();
        private static readonly AsyncLocal<Guid> span = new AsyncLocal<Guid>();
        private static readonly AsyncLocal<RequestData> request = new AsyncLocal<RequestData>();
        private static readonly AsyncLocal<Guid> session = new AsyncLocal<Guid>();

        /// <summary>
        /// The correlation trace id.
        /// </summary>
        public Guid Trace
        {
            get
            {
                if (trace.Value == Guid.Empty)
                {
                    trace.Value = Guid.NewGuid();
                }

                return trace.Value;
            }
            set
            {
                trace.Value = value;
            }
        }

        /// <summary>
        /// The correlation span id.
        /// </summary>
        public Guid Span
        {
            get
            {
                if (span.Value == Guid.Empty)
                {
                    span.Value = SpanData.Current.Id;
                }

                return span.Value;
            }
            set
            {
                span.Value = value;
                SpanData.Current.Id = value;
            }
        }

        /// <summary>
        /// The correlation request data.
        /// </summary>
        public RequestData Request
        {
            get
            {
                if (request.Value == null)
                {
                    request.Value = new RequestData();
                }

                return request.Value;
            }
            set
            {
                request.Value = value;
            }
        }

        /// <summary>
        /// The unique identifier of the current correlation session.
        /// </summary>
        public Guid Session
        {
            get
            {
                if (session.Value == Guid.Empty)
                {
                    session.Value = Guid.NewGuid();
                }

                return session.Value;
            }
            set
            {
                session.Value = value;
            }
        }

        /// <summary>
        /// This method is called to load the correlation data for the current context from a correlation request header.
        /// </summary>
        /// <param name="requestHeader">The correlation request header to load</param>
        public void Load(CorrelationRequestHeader requestHeader)
        {
            Trace = requestHeader.Trace;
            Request = new RequestData
            {
                Parent = requestHeader.Request,
                Origin = requestHeader.Span
            };
            Session = requestHeader.Session;
        }

        /// <summary>
        /// This method is called to reset the correlation data for the current context.
        /// </summary>
        public void Reset()
        {
            Trace = Guid.NewGuid();
            Span = Guid.NewGuid();
            Request = new RequestData();
            Session = Guid.NewGuid();
        }

        /// <summary>
        /// This method is called to create a correlation request header for the current context.
        /// </summary>
        public CorrelationRequestHeader CreateRequestHeader()
        {
            return new CorrelationRequestHeader
            {
                Trace = Trace,
                Span = Span,
                Request = Request.Id,
                Session = Session
            };
        }
    }
}
