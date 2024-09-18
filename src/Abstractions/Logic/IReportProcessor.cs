namespace Abstractions.Logic;

/// <summary>
/// Processor for health check reports.
/// </summary>
public interface IReportProcessor
{
    /// <summary>
    /// Processes health check reports.
    /// </summary>
    /// <param name="ct">Token for cancel the operation.</param>
    public Task ProcessAsync(CancellationToken ct);
}
