namespace Logic.Configuration
{
    /// <summary>
    /// <see cref="ReportProcessor"/> configuration.
    /// </summary>
    public sealed class ReportProcessorConfiguration
    {
        /// <summary>
        /// Healthcheck send interval.
        /// </summary>
        public required TimeSpan SendInterval { get; init; }
    }
}
