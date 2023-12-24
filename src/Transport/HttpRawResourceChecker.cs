using Abstractions;
using Models;

using Microsoft.Extensions.Logging;

namespace Transport;

/// <summary>
/// Raw transport dependent resource checker.
/// </summary>
public sealed class HttpRawResourceChecker : IRawResourceChecker
{
    /// <summary>
    /// Creates new instance of <see cref="HttpRawResourceChecker"/>.
    /// </summary>
    /// <param name="clientProxy">Http client wrapper.</param>
    /// <param name="logger">Logger.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws is any argument is null.
    /// </exception>
    public HttpRawResourceChecker(
        IHttpClientProxy clientProxy,
        ILogger<HttpRawResourceChecker> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProxy);
        ArgumentNullException.ThrowIfNull(logger);

        _clientProxy = clientProxy;
        _logger = logger;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Throws if resource argument is null.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Throws if token is expired.
    /// </exception>
    public async Task<ResourceStatus> CheckAsync(TimeSpan timeout, Uri uri, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ct.ThrowIfCancellationRequested();

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentException("Timeout must be greater than zero.", nameof(timeout));
        }

        using var client = _clientProxy.ResourceClientFactory(timeout);

        var response = await client.GetAsync(uri, ct)
                                   .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return ResourceStatus.Healthy;
        }

        return ResourceStatus.Unhealthy;
    }

    private readonly IHttpClientProxy _clientProxy;
    private readonly ILogger _logger;
}
