namespace Avvo.Core.Logging.Correlation
{
    using System;

    /// <summary>
    /// This class is used to define correlation Request Data
    /// </summary>
    public class RequestData
    {
        /// <summary>
        /// This is the unique identifier of this request.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// This is the unique identifier of this parent of this request.
        /// </summary>
        public Guid? Parent { get; set; }

        /// <summary>
        /// This is the unique identifier of this span origin of this request.
        /// </summary>
        public Guid? Origin { get; set; }

        public RequestData()
        {
            Id = Guid.NewGuid();
        }
    }
}
