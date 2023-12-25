using Models;

using Microsoft.Extensions.Logging;
using Abstractions.Transport;
using Abstractions.Logic;

namespace Logic;

/// <summary>
/// Checks the resource healthcheck.
/// </summary>
public sealed class ResourceChecker : IResourceChecker
{
    /// <summary>
    /// Creates new instance of <see cref="ResourceChecker"/>.
    /// </summary>
    /// <param name="rawChecker">raw transport dependent hc checker.</param>
    /// <param name="logger">Logger.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws if any argument is null.
    /// </exception>
    public ResourceChecker(
        IRawResourceChecker rawChecker,
        ILogger<ResourceChecker> logger)
    {
        ArgumentNullException.ThrowIfNull(rawChecker);
        ArgumentNullException.ThrowIfNull(logger);

        _rawChecker = rawChecker;
        _logger = logger;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Throws if resource argument is null.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Throws if token is expired.
    /// </exception>
    public async Task CheckAsync(ResourceHealthCheck resource, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(resource);
        ct.ThrowIfCancellationRequested();

        var status = await _rawChecker.CheckAsync(
                                resource.RequestSettings.Timeout,
                                resource.RequestSettings.Uri,
                                ct)
                           .ConfigureAwait(false);
        if (status == ResourceStatus.Healthy)
        {
            resource.Update();
        }
    }

    private readonly IRawResourceChecker _rawChecker;
    private readonly ILogger _logger;
}
