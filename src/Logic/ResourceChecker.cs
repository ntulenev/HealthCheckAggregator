using Abstractions;
using Models;

using Microsoft.Extensions.Logging;

namespace Logic;

public sealed class ResourceChecker : IResourceChecker
{
    public ResourceChecker(
        IRawResourceChecker rawChecker,
        ILogger<ResourceChecker> logger)
    {
        ArgumentNullException.ThrowIfNull(rawChecker);
        ArgumentNullException.ThrowIfNull(logger);

        _rawChecker = rawChecker;
        _logger = logger;
    }

    public async Task CheckAsync(ResourceHealthCheck resource, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(resource);
        ct.ThrowIfCancellationRequested();

        var status = await _rawChecker.CheckAsync(
                                resource.RequestSettings.Timeout,
                                resource.RequestSettings.Uri,
                                ct);
        if (status == ResourceStatus.Healthy)
        {
            resource.Update();
        }
    }

    private readonly IRawResourceChecker _rawChecker;
    private readonly ILogger<ResourceChecker> _logger;
}
