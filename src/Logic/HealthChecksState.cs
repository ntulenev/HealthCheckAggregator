using Abstractions;
using Models;

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
    /// <exception cref="ArgumentNullException">Throws if <paramref name="healthChecks"/>
    /// is null.</exception>
    public HealthChecksState(
            IEnumerable<ResourceHealthCheck> healthChecks)
    {
        ArgumentNullException.ThrowIfNull(healthChecks);

        if (!healthChecks.Any())
        {
            throw new ArgumentException("Resource health checks cannot be empty.",
                        nameof(healthChecks));
        }

        _items = healthChecks.ToFrozenSet();
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<ResourceHealthCheck> HealthChecks => _items;

    /// <inheritdoc/>
    public HealthCheckReport BuildReport() => new(_items);

    private readonly FrozenSet<ResourceHealthCheck> _items;
}

