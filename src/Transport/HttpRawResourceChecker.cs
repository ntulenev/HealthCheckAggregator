using Abstractions;
using Models;

using Microsoft.Extensions.Logging;

namespace Transport;

public sealed class HttpRawResourceChecker : IRawResourceChecker
{
    public HttpRawResourceChecker(
        IHttpClientProxy clientProxy,
        ILogger<HttpRawResourceChecker> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProxy);
        ArgumentNullException.ThrowIfNull(logger);

        _clientProxy = clientProxy;
        _logger = logger;
    }

    public async Task<ResourceStatus> CheckAsync(TimeSpan timeout, Uri uri, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ct.ThrowIfCancellationRequested();

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentException("Timeout must be greater than zero.", nameof(timeout));
        }

        using var client = _clientProxy.ResourceClientFactory(timeout);

        var response = await client.GetAsync(uri, ct);
        if (response.IsSuccessStatusCode)
        {
            return ResourceStatus.Healthy;
        }

        return ResourceStatus.Unhealthy;
    }

    private readonly IHttpClientProxy _clientProxy;
    private readonly ILogger<HttpRawResourceChecker> _logger;
}
