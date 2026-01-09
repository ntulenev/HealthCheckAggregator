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
    /// <param name="resource">Resource to check.</param>
    /// <param name="ct">Token for cancel.</param>
    Task CheckAsync(ResourceHealthCheck resource, CancellationToken ct);
}
