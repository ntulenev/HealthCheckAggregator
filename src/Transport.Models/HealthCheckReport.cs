using Newtonsoft.Json;

namespace Transport.Models;

/// <summary>
/// Aggregated healthcheck report DTO.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public sealed class HealthCheckReport
{
    /// <summary>
    /// Report creation time.
    /// </summary>
    [JsonProperty("timestamp", Required = Required.Always)]
    public required DateTimeOffset Created { get; init; }

    /// <summary>
    /// Resources in the report.
    /// </summary>
    [JsonProperty("resources", Required = Required.Always)]
    public required IEnumerable<HealthCheckReportItem> ReportItems { get; init; }
}
