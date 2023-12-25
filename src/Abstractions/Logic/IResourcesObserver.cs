namespace Abstractions.Logic;

/// <summary>
/// Observes resources health checks.
/// </summary>
public interface IResourcesObserver
{
    /// <summary>
    /// Observes resources health checks.
    /// </summary>
    /// <param name="ct">Token for cancel.</param>
    public Task ObserveAsync(CancellationToken ct);
}
