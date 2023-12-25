using Models;

namespace Abstractions.Transport;

/// <summary>
/// Raw resource checker contract.
/// </summary>
public interface IRawResourceChecker
{
    /// <summary>
    /// Checks the resource health status.
    /// </summary>
    /// <param name="timeout">Request timeout.</param>
    /// <param name="uri">Request URI.</param>
    /// <param name="ct">token for cancel.</param>
    /// <returns>Healthcheck status.</returns>
    public Task<ResourceStatus> CheckAsync(TimeSpan timeout, Uri uri, CancellationToken ct);
}
