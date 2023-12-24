﻿using Abstractions;
using Models;

using Microsoft.Extensions.Logging;
using AutoMapper;

using TModel = Transport.Models;

namespace Logic;

/// <summary>
/// Helathcheck report sender.
/// </summary>
public sealed class ReportSender
{
    /// <summary>
    /// Creates new instance of <see cref="ReportSender"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="mapper">Mapper for DTO</param>
    /// <param name="rawSender">Raw data sender.</param>
    /// <param name="serializer">Json serializer.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws if any param is null.</exception>
    public ReportSender(ILogger<ReportSender> logger,
                        IMapper mapper,
                        IRawSender<string> rawSender,
                        ISerializer<TModel.HealthCheckReport, string> serializer)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(rawSender);
        ArgumentNullException.ThrowIfNull(serializer);

        _logger = logger;
        _mapper = mapper;
        _rawSender = rawSender;
        _serializer = serializer;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Throws is report is null.</exception>
    /// <exception cref="OperationCanceledException">
    /// Thows if token is expired.</exception>
    public async Task SendAsync(HealthCheckReport report, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(report);
        ct.ThrowIfCancellationRequested();

        _logger.LogInformation("Start sedning report {@report}", report);

        var rawReport = _mapper.Map<TModel.HealthCheckReport>(report);
        var payload = _serializer.Serialize(rawReport);

        _logger.LogDebug("Payload {payload}", payload);

        await _rawSender.SendAsync(payload, ct);

        _logger.LogInformation("Report sent");
    }

    private readonly ILogger<ReportSender> _logger;
    private readonly IMapper _mapper;
    private readonly IRawSender<string> _rawSender;
    private readonly ISerializer<TModel.HealthCheckReport, string> _serializer;

}