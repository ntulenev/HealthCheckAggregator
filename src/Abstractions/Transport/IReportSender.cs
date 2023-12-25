using Models;

namespace Abstractions.Transport;

/// <summary>
/// Healthcheks report sender.
/// </summary>
public interface IReportSender
{
    /// <summary>
    /// Sends healthcheck report.
    /// </summary>
    /// <param name="report">Healthcheks report.</param>
    /// <param name="ct">Token for cancel the operation.</param>
    public Task SendAsync(HealthCheckReport report, CancellationToken ct);
}
