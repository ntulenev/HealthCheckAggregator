namespace Models;

/// <summary>
/// Healthcheck model for resource.
/// </summary>
public sealed class ResourceHealthCheck
{
    /// <summary>
    /// Resource name.
    /// </summary>
    public ResourceName ResourceName { get; }

    /// <summary>
    /// Resource settings.
    /// </summary>
    public ResourceRequestSettings RequestSettings { get; }

    /// <summary>
    /// Healthcheck last update.
    /// </summary>
    public DateTimeOffset LastUpdage { get; private set; }

    /// <summary>
    /// Healthcheck expiration period.
    /// </summary>
    public TimeSpan ExpirationPeriod { get; }

    /// <summary>
    /// Create resource healthcheck.
    /// </summary>
    /// <param name="name">Name of the resource.</param>
    /// <param name="expirationPeriod">Healthcheck expiration period.</param>
    /// <exception cref="ArgumentNullException">Throws if
    /// <paramref name="name"/> is null.</exception>
    public ResourceHealthCheck(
                ResourceName name,
                TimeSpan expirationPeriod,
                ResourceRequestSettings requestSettings)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(requestSettings);
        ResourceName = name;
        LastUpdage = DateTimeOffset.MinValue;
        ExpirationPeriod = expirationPeriod;
        RequestSettings = requestSettings;
    }

    /// <summary>
    /// Checks if healthcheck is expired.
    /// </summary>
    /// <returns>Healthcheck status.</returns>
    public ResourceStatus IsExpired()
    {
        if (LastUpdage + ExpirationPeriod >= DateTimeOffset.UtcNow)
        {
            return ResourceStatus.Healthy;
        }

        return ResourceStatus.Unhealthy;
    }

    /// <summary>
    /// Update last update time.
    /// </summary>
    public void Update()
    {
        LastUpdage = DateTimeOffset.UtcNow;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $" Resource: {ResourceName.Value} " +
            $"LastUpadate: {LastUpdage} " +
            $"ExporationPeriod: {ExpirationPeriod}";
    }
}
