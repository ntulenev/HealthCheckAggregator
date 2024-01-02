namespace Transport.Configuration;

/// <summary>
/// Report sender configuration.
/// </summary>
public sealed class ReportSenderConfiguration
{
    /// <summary>
    /// Report sender destination url.
    /// </summary>
    public required Uri Url { get; init; }

    /// <summary>
    /// Timout for generating healthcheck reports.
    /// </summary>
    public required TimeSpan Timeout { get; init; }
}
