using Abstractions;
using Models;

using Microsoft.Extensions.Logging;

namespace Transport;

public sealed class HttpResourceChecker : IResourceChecker
{
    public HttpResourceChecker(
        IHttpClientProxy clientProxy,
        ILogger<HttpResourceChecker> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProxy);
        ArgumentNullException.ThrowIfNull(logger);

        _clientProxy = clientProxy;
        _logger = logger;
    }

    public async Task CheckAsync(ResourceHealthCheck resource, CancellationToken ct)
    {
        //TODO Get from resource model.
        Uri url = null!;
        TimeSpan ts = TimeSpan.Zero;

        using var client = _clientProxy.ResourceClientFactory(ts);

        var response = await client.GetAsync(url, ct);
        if (response.IsSuccessStatusCode)
        {
            resource.Update();
        }
    }

    private readonly IHttpClientProxy _clientProxy;
    private readonly ILogger<HttpResourceChecker> _logger;
}
