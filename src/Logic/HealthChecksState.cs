using Abstractions.State;
using Logic.Configuration;
using Models;

using Microsoft.Extensions.Options;

using System.Collections.Frozen;

namespace Logic;

/// <summary>
/// Health checks state.
/// </summary>
public sealed class HealthChecksState : IHealthChecksState
{
    /// <summary>
    /// Creates <see cref="HealthChecksState"/>.
    /// </summary>
    /// <param name="config">Configuration.</param>
    /// <param name="resourceHealthCheckFactory">Factory to create resource health checks.</param>
    /// <exception cref="ArgumentException">Throws if any parameter is null.</exception>
    /// <exception cref="InvalidOperationException">Throws if resources is empty.</exception>
    public HealthChecksState(
            IOptions<HealthChecksStateConfiguration> config,
            Func<ResourceConfiguration, ResourceHealthCheck> resourceHealthCheckFactory)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(resourceHealthCheckFactory);

        var healthChecks = config.Value.Resources.Select(resourceHealthCheckFactory);
        if (!healthChecks.Any())
        {
            throw new InvalidOperationException("Resource health checks cannot be empty.");
        }
        _items = healthChecks.ToFrozenSet();
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<ResourceHealthCheck> HealthChecks => _items;

    /// <inheritdoc/>
    public HealthCheckReport BuildReport() => new(_items);

    private readonly FrozenSet<ResourceHealthCheck> _items;
}

