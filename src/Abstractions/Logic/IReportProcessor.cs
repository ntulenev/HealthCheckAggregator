namespace Abstractions.Logic;

/// <summary>
/// Processor for healthcheck reports.
/// </summary>
public interface IReportProcessor
{
    /// <summary>
    /// Processes healthcheck reports.
    /// </summary>
    /// <param name="ct">Token for cancel the operation.</param>
    public Task ProcessAsync(CancellationToken ct);
}
