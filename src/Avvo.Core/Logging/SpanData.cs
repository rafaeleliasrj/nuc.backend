namespace Avvo.Core.Logging
{
    using System.Threading;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class is used to define the details of a span.
    /// </summary>
    public class SpanData
    {
        private static readonly AsyncLocal<SpanData> current = new AsyncLocal<SpanData>();

        /// <summary>
        /// This is the span data for the current context.
        /// </summary>
        public static SpanData Current
        {
            get
            {
                if (current.Value == null)
                {
                    current.Value = new SpanData();
                }

                return current.Value;
            }
            set
            {
                current.Value = value;
            }
        }

        /// <summary>
        /// This is the unique identifier of the correlation span.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// This is the utc date time that this span was started.
        /// </summary>
        public DateTime? Started { get; set; }

        /// <summary>
        /// This is the utc date time that this span completed.
        /// </summary>
        public DateTime? Completed { get; set; }

        /// <summary>
        /// This is the duration of this span in seconds.
        /// </summary>
        public double? Duration
        {
            get
            {
                if (Started == null || Completed == null)
                {
                    return null;
                }
                TimeSpan span = (TimeSpan)(Completed - Started);
                return Math.Round(span.TotalSeconds, 4);
            }
        }

        /// <summary>
        /// This is the name of this correlation span.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This is the category of this correlation span.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// This is the sub-category of this correlation span.
        /// </summary>
        public string SubCategory { get; set; }

        /// <summary>
        /// This is the attributes for this correlation span.
        /// </summary>
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
    }
}
