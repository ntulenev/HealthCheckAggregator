namespace Abstractions;

/// <summary>
/// Process resources healthchecks.
/// </summary>
public interface IResourceCheckerProcessor
{
    /// <summary>
    /// Rusn process resources healthchecks.
    /// </summary>
    /// <param name="ct">Token for cancel.</param>
    public Task ProcessAsync(CancellationToken ct);
}
