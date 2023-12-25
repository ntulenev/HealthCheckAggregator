namespace Logic.Configuration;

public sealed class HealthChecksStateConfiguration
{
    public required IEnumerable<ResourceConfiguration> Resources { get; init; }
}
