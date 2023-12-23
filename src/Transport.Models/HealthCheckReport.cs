namespace Transport.Models;

/// <summary>
/// Aggregated healthcheck report DTO.
/// </summary>
public sealed class HealthCheckReport
{
    /// <summary>
    /// Report creation time.
    /// </summary>
    public required DateTimeOffset Created { get; init; }

    /// <summary>
    /// Resources in the report.
    /// </summary>
    public required IEnumerable<HealthCheckReportItem> ReportItems { get; init; }
}
