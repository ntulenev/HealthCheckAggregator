namespace Abstractions.Transport;

/// <summary>
/// Wrapper around <see cref="HttpClient"/>,
/// allowing the configuration of an HTTP client for a specific use case.
/// </summary>
public interface IHttpClientProxy
{
    /// <summary>
    /// Property containing a configured <see cref="HttpClient"/> for send aggregated healthcheck.
    /// </summary>
    public HttpClient SenderClient { get; }

    /// <summary>
    /// Creates containing a configured <see cref="HttpClient"/> for pull healthchecks form resources.
    /// </summary>
    public HttpClient ResourceClientFactory(TimeSpan timeout);
}
