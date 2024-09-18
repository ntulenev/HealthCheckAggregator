using Models;
namespace Abstractions.Logic;

/// <summary>
/// Checks the resource health check.
/// </summary>
public interface IResourceChecker
{
    /// <summary>
    /// Checks the resource health check.
    /// </summary>
    /// <param name="resouce">Resource to check.</param></param>
    /// <param name="ct">Token for cancel.</param>
    public Task CheckAsync(ResourceHealthCheck resouce, CancellationToken ct);
}
