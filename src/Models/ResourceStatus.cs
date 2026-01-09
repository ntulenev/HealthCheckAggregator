namespace Models;

/// <summary>
/// Resource status.
/// </summary>
public enum ResourceStatus
{
    /// <summary>
    /// Indicates that the health status is unhealthy.
    /// </summary>
    Unhealthy = 0,
    /// <summary>
    /// Indicates that the component is healthy and operating as expected.
    /// </summary>
    Healthy = 1
}
