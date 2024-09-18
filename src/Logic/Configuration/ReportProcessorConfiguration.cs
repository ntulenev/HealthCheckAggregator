namespace Logic.Configuration
{
    /// <summary>
    /// <see cref="ReportProcessor"/> configuration.
    /// </summary>
    public sealed class ReportProcessorConfiguration
    {
        /// <summary>
        /// Health check send interval.
        /// </summary>
        public required TimeSpan SendInterval { get; init; }
    }
}
