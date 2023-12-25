namespace Logic.Configuration;

public sealed class ResourceConfiguration
{
    public required string Name { get; init; }
    public required TimeSpan ExpirationPeriod { get; init; }
    public required Uri Uri { get; init; }
    public required TimeSpan CheckInterval { get; init; }
    public required TimeSpan Timeout { get; init; }
}
