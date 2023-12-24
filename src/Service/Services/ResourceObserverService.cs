using Abstractions;

namespace Service.Services
{
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) =>
                                        await _resourcesObserver.ObserveAsync(stoppingToken)
                                                                .ConfigureAwait(false);

        private readonly IResourcesObserver _resourcesObserver;
    }
}
