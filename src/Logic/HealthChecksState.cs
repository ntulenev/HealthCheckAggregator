using Abstractions;
using Models;

using Microsoft.Extensions.Logging;

using System.Collections.Frozen;

namespace Logic;

/// <summary>
/// Healthchecks state.
/// </summary>
public sealed class HealthChecksState : IHealthChecksState
{
    /// <summary>
    /// Creates <see cref="HealthChecksState"/>.
    /// </summary>
    /// <param name="healthChecks">Resources to be tracked with healthcheks.</param>
    /// <exception cref="ArgumentException">Throws if <paramref name="healthChecks"/>
    /// is empty.</exception>
    /// <exception cref="ArgumentNullException">Throws if any parameter
    /// is null.</exception>
    public HealthChecksState(
            ILogger<HealthChecksState> logger,
            IEnumerable<ResourceHealthCheck> healthChecks)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(healthChecks);

        if (!healthChecks.Any())
        {
            throw new ArgumentException("Resource health checks cannot be empty.",
                        nameof(healthChecks));
        }

        _items = healthChecks.ToFrozenDictionary(x => x.ResourceName);
        _logger = logger;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if resource is not 
    /// registered in state.</exception>
    /// <exception cref="ArgumentNullException">Throws if resource
    /// is null.</exception>
    public void Update(ResourceName resourceName)
    {
        ArgumentNullException.ThrowIfNull(resourceName);

        _logger.LogDebug("Try to find resource {resource}", resourceName);

        if (_items.TryGetValue(resourceName, out var hc))
        {
            _logger.LogInformation("Updating HC Timestamp for {resource}", resourceName);
            hc.Update();
        }
        else
        {
            _logger.LogError("Resource not found {resource}", resourceName);

            throw new ArgumentException("Resource is not registered.",
                                       nameof(resourceName));
        }
    }

    /// <inheritdoc/>
    public HealthCheckReport BuildReport() => new(_items.Values);

    private readonly FrozenDictionary<ResourceName, ResourceHealthCheck> _items;
    private readonly ILogger _logger;
}

