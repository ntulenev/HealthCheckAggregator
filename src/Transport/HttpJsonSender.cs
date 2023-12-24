using Abstractions;

using System.Text;

namespace Transport;

/// <summary>
/// Http Sender that sends JSON data to the destination.
/// </summary>
public sealed class HttpJsonSender : IRawSender<string>
{
    /// <summary>
    /// Creates a new instance of <see cref="HttpJsonSender"/>.
    /// </summary>
    /// <param name="clientProxy">Http client wrapper.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws if clientProxy is null.</exception>
    public HttpJsonSender(IHttpClientProxy clientProxy)
    {
        ArgumentNullException.ThrowIfNull(clientProxy);
        _clientProxy = clientProxy;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Throws is data for uri is null.</exception>
    /// <exception cref="OperationCanceledException">
    /// Throws if token is expired.</exception>
    public async Task SendAsync(string data, Uri uri, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(data);
        ct.ThrowIfCancellationRequested();

        using var content =
           new StringContent(
           data,
           Encoding.UTF8,
           CONTENT_TYPE);

        await _clientProxy.SenderClient.PostAsync(uri, content, ct)
                          .ConfigureAwait(false);
    }

    private const string CONTENT_TYPE = "application/json";
    private readonly IHttpClientProxy _clientProxy;
}
