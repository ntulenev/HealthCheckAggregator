using Abstractions.Transport;

namespace Transport;

/// <summary>
/// Provides <see cref="HttpClient"/> for request and response HC logic.
/// </summary>
public sealed class HttpClientProxy : IHttpClientProxy
{
    /// <summary>
    /// Creates a new instance of <see cref="HttpClientProxy"/>.
    /// </summary>
    /// <param name="responseHttpClient">Single client for sending aggregated HC.</param>
    /// <param name="resourceClientFactory">Factory to create request clients.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws is any of the parameters is null.
    /// </exception>
    public HttpClientProxy(HttpClient responseHttpClient,
                           Func<TimeSpan, HttpClient> resourceClientFactory)
    {
        ArgumentNullException.ThrowIfNull(responseHttpClient);
        ArgumentNullException.ThrowIfNull(resourceClientFactory);

        _httpClient = responseHttpClient;
        _resourceClientFactory = resourceClientFactory;
    }

    /// <inheritdoc/>
    public HttpClient SenderClient => _httpClient;

    /// <inheritdoc/>
    public HttpClient ResourceClientFactory(TimeSpan timeout)
        => _resourceClientFactory(timeout);

    private readonly HttpClient _httpClient;
    private readonly Func<TimeSpan, HttpClient> _resourceClientFactory;
}
