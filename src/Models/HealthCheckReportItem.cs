namespace Models;

/// <summary>
/// Report item about resource healthcheck.
/// </summary>
public sealed class HealthCheckReportItem
{
    /// <summary>
    /// Resournce name.
    /// </summary>
    public ResourceName ResourceName { get; }

    /// <summary>
    /// Resource status.
    /// </summary>
    public ResourceStatus Status { get; }

    /// <summary>
    /// Create report item.
    /// </summary>
    /// <param name="resourceName">Resource name.</param>
    /// <param name="status">Resource status.</param>
    /// <exception cref="ArgumentException">Throws is enum is not defined.</exception>
    /// <exception cref="ArgumentNullException">Throws if resourceName is null.</exception>
    public HealthCheckReportItem(ResourceName resourceName, ResourceStatus status)
    {
        ArgumentNullException.ThrowIfNull(resourceName);

        if (!Enum.IsDefined(typeof(ResourceStatus), status))
        {
            throw new ArgumentException("Status is not defined.", nameof(status));
        }

        ResourceName = resourceName;
        Status = status;
    }
}
