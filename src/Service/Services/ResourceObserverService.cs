using Abstractions.Logic;

namespace Service.Services;

/// <summary>
/// Service for observing resources health checks.
/// </summary>
public sealed class ResourceObserverService : BackgroundService
{
    /// <summary>
    /// Creates <see cref="ResourceObserverService"/>.
    /// </summary>
    /// <param name="resourceObserver">Resources observer.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws exception is <paramref name="resourceObserver"/> is null.</exception>
    public ResourceObserverService(IResourcesObserver resourceObserver)
    {
        ArgumentNullException.ThrowIfNull(resourceObserver);
        _resourcesObserver = resourceObserver;
    }

    /// <summary>
    /// Executes the background service operation asynchronously and observes resource changes until the service is
    /// stopped.
    /// </summary>
    /// <param name="stoppingToken">A cancellation token that is triggered when the host is performing a graceful shutdown. The operation should
    /// monitor this token to stop execution promptly when cancellation is requested.</param>
    /// <returns>A task that represents the asynchronous execution of the background service operation.</returns>
    protected async override Task ExecuteAsync(CancellationToken stoppingToken) =>
                                    await _resourcesObserver.ObserveAsync(stoppingToken)
                                                            .ConfigureAwait(false);

    private readonly IResourcesObserver _resourcesObserver;
}
