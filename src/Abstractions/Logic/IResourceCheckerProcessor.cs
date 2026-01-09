namespace Abstractions.Logic;

/// <summary>
/// Process resources health checks.
/// </summary>
public interface IResourceCheckerProcessor
{
    /// <summary>
    /// Run process resources health checks.
    /// </summary>
    /// <param name="ct">Token for cancel.</param>
    Task ProcessAsync(CancellationToken ct);
}
