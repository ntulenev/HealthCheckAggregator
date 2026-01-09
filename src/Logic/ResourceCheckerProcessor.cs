using Models;
using Abstractions.Logic;

using Microsoft.Extensions.Logging;

namespace Logic;

/// <summary>
/// Checks the resource health check.
/// </summary>
public sealed class ResourceCheckerProcessor : IResourceCheckerProcessor
{
    /// <summary>
    /// Creates new instance of <see cref="ResourceCheckerProcessor"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="resourceChecker">Resource checker.</param>
    /// <param name="resourceHealthCheck">Target resource.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws if any argument is null.
    /// </exception>
    public ResourceCheckerProcessor(ILogger<ResourceCheckerProcessor> logger,
                                    IResourceChecker resourceChecker,
                                    ResourceHealthCheck resourceHealthCheck)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(resourceChecker);
        ArgumentNullException.ThrowIfNull(resourceHealthCheck);

        _logger = logger;
        _resourceChecker = resourceChecker;
        _resourceHealthCheck = resourceHealthCheck;
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">
    /// Throw if token is expired.
    /// </exception>
    public async Task ProcessAsync(CancellationToken ct)
    {
        while (true)
        {
            using var _ = _logger.BeginScope("Resource {Resource}.", _resourceHealthCheck.ResourceName);

            ct.ThrowIfCancellationRequested();

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {


                await _resourceChecker.CheckAsync(_resourceHealthCheck, ct)
                                      .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error while checking resource {Resource}.", _resourceHealthCheck.ResourceName);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            await Task.Delay(_resourceHealthCheck.RequestSettings.CheckInterval, ct)
                      .ConfigureAwait(false);
        }
    }

    private readonly ILogger _logger;
    private readonly IResourceChecker _resourceChecker;
    private readonly ResourceHealthCheck _resourceHealthCheck;
}
