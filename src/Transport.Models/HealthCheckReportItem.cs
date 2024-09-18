using Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Transport.Models;

/// <summary>
/// Report item about resource health check DTO.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public sealed class HealthCheckReportItem
{
    /// <summary>
    /// Resource name.
    /// </summary>
    [JsonProperty("resource_name", Required = Required.Always)]
    public required string ResourceName { get; init; }

    /// <summary>
    /// Resource status.
    /// </summary>
    [JsonProperty("resource_status", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public required ResourceStatus Status { get; init; }
}
