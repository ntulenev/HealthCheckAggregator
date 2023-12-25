namespace Models;

/// <summary>
/// Resource settings for request hc.
/// </summary>
public sealed class ResourceRequestSettings
{
    /// <summary>
    /// Url of the resource.
    /// </summary>
    public Uri Url { get; }

    /// <summary>
    /// Check interval.
    /// </summary>
    public TimeSpan CheckInterval { get; }

    /// <summary>
    /// Request timeout.
    /// </summary>
    public TimeSpan Timeout { get; }

    /// <summary>
    /// Creates <see cref="ResourceRequestSettings"/>.
    /// </summary>
    /// <param name="url">Url.</param>
    /// <param name="checkInterval">Healthcheck interval.</param>
    /// <param name="timeout">Request timeout.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws if url is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if
    /// <paramref name="checkInterval"/> or <paramref name="timeout"/> less or equals to zero.</exception>
    public ResourceRequestSettings(
        Uri url,
        TimeSpan checkInterval,
        TimeSpan timeout)
    {

        ArgumentNullException.ThrowIfNull(url);

        if (checkInterval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(checkInterval), "Check interval must be greater than zero.");
        }

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be greater than zero.");
        }

        Url = url;
        CheckInterval = checkInterval;
        Timeout = timeout;
    }
}
