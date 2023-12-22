using Models;

namespace Abstractions;

/// <summary>
/// Contract for healthchecks state.
/// </summary>
public interface IHealthChecksState
{
    /// <summary>
    /// Updates healthcheck timestamp for the resource.
    /// </summary>
    /// <param name="resourceName"></param>
    public void Update(ResourceName resourceName);

    /// <summary>
    /// Builds aggregated healthcheck report about all resources.
    /// </summary>
    /// <returns></returns>
    public HealthCheckReport BuildReport();
}
