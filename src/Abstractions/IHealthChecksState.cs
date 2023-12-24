using Models;
using System.Collections.Frozen;

namespace Abstractions;

/// <summary>
/// Contract for healthchecks state.
/// </summary>
public interface IHealthChecksState
{

    /// <summary>
    /// Gets healthchecks.
    /// </summary>
    /// <value>Healthchecks.</value>
    public IReadOnlyCollection<ResourceHealthCheck> HealthChecks { get; }

    /// <summary>
    /// Builds aggregated healthcheck report about all resources.
    /// </summary>
    /// <returns></returns>
    public HealthCheckReport BuildReport();
}
