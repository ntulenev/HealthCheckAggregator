namespace Transport.Configuration;

/// <summary>
/// Report sender configuration.
/// </summary>
public sealed class ReportSenderConfiguration
{
    /// <summary>
    /// Report sender destination url.
    /// </summary>
    public required string Url { get; init; }
}
