using Models;
using Abstractions.Transport;

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
    public async Task<ResourceStatus> CheckAsync(TimeSpan timeout, Uri url, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(url);
        ct.ThrowIfCancellationRequested();

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentException("Timeout must be greater than zero.", nameof(timeout));
        }

        using var client = _clientProxy.ResourceClientFactory(timeout);

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var response = await client.GetAsync(url, ct)
                                       .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return ResourceStatus.Healthy;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while checking resource {Url}.", url);
        }
#pragma warning restore CA1031 // Do not catch general exception types

        return ResourceStatus.Unhealthy;
    }

    private readonly IHttpClientProxy _clientProxy;
    private readonly ILogger _logger;
}
