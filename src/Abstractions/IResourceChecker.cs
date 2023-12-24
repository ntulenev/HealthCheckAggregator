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
    public Task CheckAsync(CancellationToken ct);
}
