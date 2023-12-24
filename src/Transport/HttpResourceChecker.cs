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
        //TODO Extract and mode to transport 
        //TODO This class move to Logic
        using var client = _clientProxy.ResourceClientFactory(resource.RequestSettings.Timeout);

        var response = await client.GetAsync(resource.RequestSettings.Uri, ct);
        if (response.IsSuccessStatusCode)
        {
            resource.Update();
        }
    }

    private readonly IHttpClientProxy _clientProxy;
    private readonly ILogger<HttpResourceChecker> _logger;
}
