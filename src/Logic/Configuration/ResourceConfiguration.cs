namespace Logic.Configuration;

/// <summary>
/// Resource configuration.
/// </summary>
public sealed class ResourceConfiguration
{
    /// <summary>
    /// Resource name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Resource Healthcheck expiration time.
    /// </summary>
    public required TimeSpan ExpirationPeriod { get; init; }

    /// <summary>
    /// Resource address.
    /// </summary>
    public required Uri Url { get; init; }

    /// <summary>
    /// Resource Healthcheck check interval.
    /// </summary>
    public required TimeSpan CheckInterval { get; init; }

    /// <summary>
    /// Resource Healthcheck timeout.
    /// </summary>
    public required TimeSpan Timeout { get; init; }
}
