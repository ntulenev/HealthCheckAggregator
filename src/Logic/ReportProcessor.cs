﻿using Abstractions;

using Logic.Configuration;

using Microsoft.Extensions.Logging;

namespace Logic;

/// <summary>
/// Processor that periodically sends healthcheck reports.
/// </summary>
public class ReportProcessor : IReportProcessor
{
    /// <summary>
    /// Creates new instance of <see cref="ReportProcessor"/>.
    /// </summary>
    /// <param name="config">Processig config.</param>
    /// <param name="logger">Processor logger.</param>
    /// <param name="healthChecksState">Healthchecks state.</param>
    /// <param name="reportSender">Report sender.</param>
    /// <exception cref="ArgumentNullException">Throws if any of 
    /// params is null.</exception>
    public ReportProcessor(
               ReportProcessorConfiguration config,
               ILogger<ReportProcessor> logger,
               IHealthChecksState healthChecksState,
               IReportSender reportSender)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(healthChecksState);
        ArgumentNullException.ThrowIfNull(reportSender);

        _config = config;
        _logger = logger;
        _healthChecksState = healthChecksState;
        _reportSender = reportSender;
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">
    /// Throws if cancellation requested.
    /// </exception>
    public async Task ProcessAsync(CancellationToken ct)
    {
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            _logger.BeginScope("Report processing");
            _logger.LogInformation("Start processing report");
            _logger.LogDebug("Building report");
            var report = _healthChecksState.BuildReport();
            _logger.LogDebug("Sending report {@report}", report);
            await _reportSender.SendAsync(report, ct).ConfigureAwait(false);
            _logger.LogInformation("Report has beeen sended.");
            await Task.Delay(_config.SendInterval, ct).ConfigureAwait(false);
        }
    }

    private readonly ReportProcessorConfiguration _config;
    private readonly ILogger _logger;
    private readonly IHealthChecksState _healthChecksState;
    private readonly IReportSender _reportSender;
}
