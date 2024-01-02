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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
                                => await _reportProcessor.ProcessAsync(stoppingToken)
                                                         .ConfigureAwait(false);

    private readonly IReportProcessor _reportProcessor;
}
