namespace Models;

/// <summary>
/// Health check model for resource.
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
    /// Health check last update.
    /// </summary>
    public DateTimeOffset LastUpdate {
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
    /// Health check expiration period.
    /// </summary>
    public TimeSpan ExpirationPeriod { get; }

    /// <summary>
    /// Create resource health check.
    /// </summary>
    /// <param name="name">Name of the resource.</param>
    /// <param name="expirationPeriod">Health check expiration period.</param>
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
    /// Checks if health check is expired.
    /// </summary>
    /// <returns>Health check status.</returns>
    public ResourceStatus IsExpired()
    {
        return (LastUpdate + ExpirationPeriod >= DateTimeOffset.UtcNow)
            ? ResourceStatus.Healthy
            : ResourceStatus.Unhealthy;
    }

    /// <summary>
    /// Update last update time.
    /// </summary>
    public void Update() =>
        //Inform the compiler and the runtime that this variable can be used
        //by multiple threads, and this prevents the compiler or the CPU from applying
        //optimizations like caching this variable locally.
        _ = Interlocked.Exchange(ref _dateTicks, DateTimeOffset.UtcNow.Ticks);

    /// <inheritdoc/>
    public override string ToString()
    {
        return $" Resource: {ResourceName.Value} " +
            $"LastUpdate: {LastUpdate} " +
            $"ExpirationPeriod: {ExpirationPeriod}";
    }

    private long _dateTicks;
}

