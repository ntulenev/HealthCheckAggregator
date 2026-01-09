using Models;

namespace Abstractions.State;

/// <summary>
/// Contract for health checks state.
/// </summary>
public interface IHealthChecksState
{

    /// <summary>
    /// Gets health checks.
    /// </summary>
    /// <value>Health checks.</value>
    IReadOnlyCollection<ResourceHealthCheck> HealthChecks { get; }

    /// <summary>
    /// Builds aggregated health check report about all resources.
    /// </summary>
    /// <returns></returns>
    HealthCheckReport BuildReport();
}
