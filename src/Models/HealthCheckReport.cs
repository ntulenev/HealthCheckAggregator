namespace Models;

/// <summary>
/// Aggregated healthcheck report.
/// </summary>
public sealed class HealthCheckReport
{
    /// <summary>
    /// Report creation time.
    /// </summary>
    public DateTimeOffset Created { get; }

    /// <summary>
    /// Resources in the report.
    /// </summary>
    public IReadOnlyCollection<HealthCheckReportItem> ReportItems => _reportItems;

    /// <summary>
    /// Creates <see cref="HealthCheckReport"/>.
    /// </summary>
    /// <param name="resourceHealthChecks">Report items.</param>
    /// <exception cref="ArgumentException">Throws if <paramref name="resourceHealthChecks"/> 
    /// does not contains any elements.</exception>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="resourceHealthChecks"/>
    /// is null.</exception>
    public HealthCheckReport(IEnumerable<ResourceHealthCheck> resourceHealthChecks)
    {
        ArgumentNullException.ThrowIfNull(resourceHealthChecks);

        if (!resourceHealthChecks.Any())
        {
            throw new ArgumentException("Resource health checks cannot be empty.",
                        nameof(resourceHealthChecks));
        }

        _reportItems = resourceHealthChecks.Select(
                                x => new HealthCheckReportItem(x.ResourceName, x.IsExpired()))
                                .ToList();

        Created = DateTimeOffset.UtcNow;
    }

    private readonly List<HealthCheckReportItem> _reportItems;
}
