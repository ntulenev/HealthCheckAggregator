using Abstractions.Logic;

namespace Service.Services;

/// <summary>
/// Service that sends healthcheck reports.
/// </summary>
public sealed class SenderService : BackgroundService
{
    /// <summary>
    /// Creates <see cref="SenderService"/>.
    /// </summary>
    /// <param name="reportProcessor">Health checks report processor.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws if <paramref name="reportProcessor"/> is null.
    /// </exception>
    public SenderService(IReportProcessor reportProcessor)
    {
        ArgumentNullException.ThrowIfNull(reportProcessor);
        _reportProcessor = reportProcessor;
    }

    /// <summary>
    /// Executes the background service operation asynchronously.
    /// </summary>
    /// <param name="stoppingToken">A cancellation token that can be used to signal the request to stop the operation.</param>
    /// <returns>A task that represents the asynchronous execution operation.</returns>
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
                                => await _reportProcessor.ProcessAsync(stoppingToken)
                                                         .ConfigureAwait(false);

    private readonly IReportProcessor _reportProcessor;
}
