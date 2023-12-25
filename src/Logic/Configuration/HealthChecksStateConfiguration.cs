namespace Logic.Configuration;

/// <summary>
/// Configuration for <see cref="HealthChecksState"/>.
/// </summary>
public sealed class HealthChecksStateConfiguration
{
    /// <summary>
    /// Resources configurations.
    /// </summary>
    public required IEnumerable<ResourceConfiguration> Resources { get; init; }
}
