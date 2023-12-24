namespace Models;

public sealed class ResourceRequestSettings
{
    public Uri Uri { get; }
    public TimeSpan CheckInterval { get; }
    public TimeSpan Timeout { get; }

    public ResourceRequestSettings(
        Uri uri,
        TimeSpan checkInterval,
        TimeSpan timeout)
    {

        ArgumentNullException.ThrowIfNull(uri);

        if (checkInterval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(checkInterval), "Check interval must be greater than zero.");
        }

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be greater than zero.");
        }

        Uri = uri;
        CheckInterval = checkInterval;
        Timeout = timeout;
    }
}
