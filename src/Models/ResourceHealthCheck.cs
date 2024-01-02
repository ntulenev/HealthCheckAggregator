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
    public DateTimeOffset LastUpdate
    {
        get
        {
            //Inform the compiler and the runtime that this variable can be used
            //by multiple threads, and this prevents the compiler or the CPU from applying
            //optimizations like caching this variable locally.
            var storedTicks = Volatile.Read(ref _dateTicks);
            return new DateTimeOffset(storedTicks, TimeSpan.Zero);
        }
    }

    /// <summary>
    /// Healthcheck expiration period.
    /// </summary>
    public TimeSpan ExpirationPeriod { get; }

    /// <summary>
    /// Create resource healthcheck.
    /// </summary>
    /// <param name="name">Name of the resource.</param>
    /// <param name="expirationPeriod">Healthcheck expiration period.</param>
    /// <param name="requestSettings">Settings to operate with resource.</param>
    /// <exception cref="ArgumentNullException">Throws if
    /// <paramref name="name"/> is null.</exception>
    public ResourceHealthCheck(
                ResourceName name,
                TimeSpan expirationPeriod,
                ResourceRequestSettings requestSettings)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(requestSettings);
        
        _dateTicks = DateTimeOffset.MinValue.Ticks;
        ResourceName = name;
        ExpirationPeriod = expirationPeriod;
        RequestSettings = requestSettings;
    }

    /// <summary>
    /// Checks if healthcheck is expired.
    /// </summary>
    /// <returns>Healthcheck status.</returns>
    public ResourceStatus IsExpired()
    {
        if (LastUpdate + ExpirationPeriod >= DateTimeOffset.UtcNow)
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
        //Inform the compiler and the runtime that this variable can be used
        //by multiple threads, and this prevents the compiler or the CPU from applying
        //optimizations like caching this variable locally.
        _ = Interlocked.Exchange(ref _dateTicks, DateTimeOffset.UtcNow.Ticks);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $" Resource: {ResourceName.Value} " +
            $"LastUpdate: {LastUpdate} " +
            $"ExpirationPeriod: {ExpirationPeriod}";
    }

    private long _dateTicks;
}

