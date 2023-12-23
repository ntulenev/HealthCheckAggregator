using Models;

using Newtonsoft.Json;

namespace Transport.Models;

/// <summary>
/// Report item about resource healthcheck DTO.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public sealed class HealthCheckReportItem
{
    /// <summary>
    /// Resournce name.
    /// </summary>
    [JsonProperty("resource_name", Required = Required.Always)]
    public required string ResourceName { get; init; }

    /// <summary>
    /// Resource status.
    /// </summary>
    [JsonProperty("resource_status", Required = Required.Always)]
    public required ResourceStatus Status { get; init; }
}
