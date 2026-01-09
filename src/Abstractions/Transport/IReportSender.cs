using Models;

namespace Abstractions.Transport;

/// <summary>
/// Health checks report sender.
/// </summary>
public interface IReportSender
{
    /// <summary>
    /// Sends health check report.
    /// </summary>
    /// <param name="report">Health checks report.</param>
    /// <param name="ct">Token for cancel the operation.</param>
    Task SendAsync(HealthCheckReport report, CancellationToken ct);
}
