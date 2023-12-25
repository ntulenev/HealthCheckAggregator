using Abstractions.Logic;
using Abstractions.State;
using Models;

using Microsoft.Extensions.Logging;

namespace Logic;

/// <summary>
/// Observes resources healthchecks.
/// </summary>
public sealed class ResourcesObserver : IResourcesObserver
{
    /// <summary>
    /// Creates new instance of <see cref="ResourcesObserver"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="state">Resources internal state.</param>
    /// <param name="processorFactory">Factory method to create resource processor.</param>
    public ResourcesObserver(ILogger<ResourcesObserver> logger,
                             IHealthChecksState state,
                             Func<ResourceHealthCheck, IResourceCheckerProcessor> processorFactory)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(processorFactory);

        _logger = logger;
        _state = state;
        _processorFactory = processorFactory;
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">
    /// Throws if <paramref name="ct"/> is expired.
    /// </exception>
    public async Task ObserveAsync(CancellationToken ct)
    {
        _logger.LogDebug("Starting resources observer.");

        ct.ThrowIfCancellationRequested();

        await Task.WhenAll(_state.HealthChecks
                                 .Select(_processorFactory)
                                 .Select(x => x.ProcessAsync(ct)))
                                 .ConfigureAwait(false);
    }

    private readonly IHealthChecksState _state;
    private readonly ILogger _logger;
    private readonly Func<ResourceHealthCheck, IResourceCheckerProcessor> _processorFactory;
}
