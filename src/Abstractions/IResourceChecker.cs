using Models;

namespace Abstractions;

/// <summary>
/// Checks the resource healthcheck.
/// </summary>
public interface IResourceChecker
{
    /// <summary>
    /// Checks the resource healthcheck.
    /// </summary>
    /// <param name="ct">Token for cancel.</param>
    /// <returns>Status of HC.</returns>
    public Task CheckAsync(ResourceHealthCheck resouce, CancellationToken ct);
}
