namespace Abstractions.Transport;

/// <summary>
/// Wrapper around <see cref="HttpClient"/>,
/// allowing the configuration of an HTTP client for a specific use case.
/// </summary>
public interface IHttpClientProxy
{
    /// <summary>
    /// Property containing a configured <see cref="HttpClient"/> for send aggregated health check.
    /// </summary>
    HttpClient SenderClient { get; }

    /// <summary>
    /// Creates containing a configured <see cref="HttpClient"/> for pull health checks form resources.
    /// </summary>
    HttpClient ResourceClientFactory(TimeSpan timeout);
}
