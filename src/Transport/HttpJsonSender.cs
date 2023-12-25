using Abstractions.Transport;

using Microsoft.Extensions.Logging;

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
    /// Throws if any parameter is null.</exception>
    public HttpJsonSender(IHttpClientProxy clientProxy,
                          ILogger<HttpJsonSender> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProxy);
        ArgumentNullException.ThrowIfNull(logger);
        _clientProxy = clientProxy;
        _logger = logger;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Throws is data for uri is null.</exception>
    /// <exception cref="OperationCanceledException">
    /// Throws if token is expired.</exception>
    public async Task<bool> SendAsync(string data, Uri uri, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(data);
        ct.ThrowIfCancellationRequested();

        using var content =
           new StringContent(
           data,
           Encoding.UTF8,
           CONTENT_TYPE);

        try
        {
            var result = await _clientProxy.SenderClient.PostAsync(uri, content, ct)
                              .ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                _logger.LogWarning("Failed to send data to {uri}. Status code: {code}. Reason: {reason}",
                                  uri,
                                  result.StatusCode,
                                  result.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send data to {uri}.", uri);
        }

        return false;
    }

    private const string CONTENT_TYPE = "application/json";
    private readonly IHttpClientProxy _clientProxy;
    private readonly ILogger _logger;
}
