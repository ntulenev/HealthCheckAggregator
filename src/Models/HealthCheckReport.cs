namespace Models;

/// <summary>
/// Aggregated health check report.
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
    /// Check report status.
    /// </summary>
    public bool IsUnhealthy => _reportItems.All(x => x.Status == ResourceStatus.Unhealthy);

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

#pragma warning disable CA1851 // Possible multiple enumerations of 'IEnumerable' collection
        if (!resourceHealthChecks.Any())
        {
            throw new ArgumentException("Resource health checks cannot be empty.",
                        nameof(resourceHealthChecks));
        }
#pragma warning restore CA1851 // Possible multiple enumerations of 'IEnumerable' collection

#pragma warning disable CA1851 // Possible multiple enumerations of 'IEnumerable' collection
        _reportItems = [.. resourceHealthChecks.Select(
                                x => new HealthCheckReportItem(x.ResourceName, x.IsExpired()))];
#pragma warning restore CA1851 // Possible multiple enumerations of 'IEnumerable' collection

        Created = DateTimeOffset.UtcNow;
    }

    private readonly List<HealthCheckReportItem> _reportItems;
}
