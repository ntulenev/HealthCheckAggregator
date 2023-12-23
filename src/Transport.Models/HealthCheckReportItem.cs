using Models;

namespace Transport.Models;

/// <summary>
/// Report item about resource healthcheck DTO.
/// </summary>
public sealed class HealthCheckReportItem
{
    /// <summary>
    /// Resournce name.
    /// </summary>
    public required string ResourceName { get; init; }

    /// <summary>
    /// Resource status.
    /// </summary>
    public required ResourceStatus Status { get; init; }
}
