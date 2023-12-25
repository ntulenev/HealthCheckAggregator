﻿namespace Abstractions.Transport;

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
    /// <param name="uri">Destination uri.</param>
    /// <param name="ct">Token for cancel the operation.</param>
    public Task<bool> SendAsync(TData data, Uri uri, CancellationToken ct);
}
