namespace Abstractions;

/// <summary>
/// Wrapper around <see cref="System.Net.Http.HttpClient"/>,
/// allowing the configuration of an HTTP client for a specific use case.
/// </summary>
public interface IHttpClientProxy
{
    /// <summary>
    /// Property containing a configured <see cref="HttpClient"/>.
    /// </summary>
    public HttpClient Client { get; }
}
