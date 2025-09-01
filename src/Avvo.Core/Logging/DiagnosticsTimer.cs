namespace Avvo.Core.Logging
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public static class DiagnosticsTimer
    {
        private static readonly AsyncLocal<ConcurrentBag<DiagnosticsTimerRecord>> records = new AsyncLocal<ConcurrentBag<DiagnosticsTimerRecord>>();

        /// <summary>
        /// This property is called to determine if the DiagnosticsTimer is recording for the current context.
        /// </summary>
        public static bool IsRecording { get { return records.Value != null; } }

        /// <summary>
        /// This property is called to return the total duration recorded so far
        /// </summary>
        public static double? TotalDuration { get { return records.Value?.Sum(s => s.Duration); } }

        /// <summary>
        /// This method is called to record the duration of a diagnostics timings by category/subcategroy
        /// </summary>
        /// <param name="duration">The duration in seconds</param>
        /// <param name="category">The category of the recording</param>
        /// <param name="subCategory">The subcategory of the recording</param>
        public static void RecordDuration(double duration, string category, string? subCategory = null)
        {
            if (records.Value == null)
            {
                return;
            }

            records.Value.Add(new DiagnosticsTimerRecord { Category = category, SubCategory = subCategory, Duration = duration });
        }

        /// <summary>
        /// This method is called to start recording diagnostics timings
        /// </summary>
        public static void StartRecording()
        {
            records.Value = new ConcurrentBag<DiagnosticsTimerRecord>();
        }

        /// <summary>
        /// This method is called to stop recording diagnostics timings and return the results.
        /// </summary>
        public static object StopRecording()
        {
            Dictionary<string, object> results = new Dictionary<string, object>();

            var safeRecords = records.Value ?? Enumerable.Empty<DiagnosticsTimerRecord>();
            var categories = safeRecords.Select(r => r.Category).Distinct();
            foreach (string? category in categories)
            {
                var categoryResult = new Dictionary<string, object>
                {
                    { "All", safeRecords.Where(r => r.Category == category).Sum(r => r.Duration) }
                };

                var subCategories = safeRecords.Where(r => r.Category == category).Select(r => r.SubCategory).Distinct();
                foreach (var subCategory in from subCategory in subCategories
                                            where !string.IsNullOrEmpty(subCategory)
                                            select subCategory)
                {
                    categoryResult[subCategory] = safeRecords.Where(r => r.Category == category && r.SubCategory == subCategory).Sum(r => r.Duration);
                }

                if (!string.IsNullOrEmpty(category))
                    results.Add(category, categoryResult);
            }

            records.Value = null!;

            return results;
        }
    }
}
