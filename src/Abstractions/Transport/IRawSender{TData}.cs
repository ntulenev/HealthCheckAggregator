namespace Abstractions.Transport;

/// <summary>
/// Send data to the destination.
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface IRawSender<TData>
{
    /// <summary>
    /// Sends data.
    /// </summary>
    /// <param name="data">Data to send.</param>
    /// <param name="url">Destination url.</param>
    /// <param name="ct">Token for cancel the operation.</param>
    Task<bool> SendAsync(TData data, Uri url, CancellationToken ct);
}
