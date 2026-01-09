using FluentAssertions;

using Newtonsoft.Json;

using Models;

namespace Transport.Models.Tests;

public class HealthCheckReportTests
{
    [Fact(DisplayName = $"{nameof(HealthCheckReportTests)} could be serialized and deserialized")]
    [Trait("Category", "Unit")]
    public void JsonSerializationDeserializationSuccess()
    {
        // Arrange
        var created = DateTimeOffset.UtcNow;
        var reportItems = new List<HealthCheckReportItem>
        {
            new()
            {
                ResourceName = "Resource1",
                Status = ResourceStatus.Healthy
            },
            new()
            {
                ResourceName = "Resource2",
                Status = ResourceStatus.Unhealthy
            }
        };

        var report = new HealthCheckReport
        {
            Created = created,
            ReportItems = reportItems
        };

        // Act
        var json = JsonConvert.SerializeObject(report);
        var deserializedReport = JsonConvert.DeserializeObject<HealthCheckReport>(json);

        // Assert
        deserializedReport.Should().NotBeNull();
        deserializedReport!.Created.Should().Be(created);
        deserializedReport.ReportItems.Should().BeEquivalentTo(reportItems);
    }

    [Fact(DisplayName = $"{nameof(HealthCheckReportTests)} can be deserialized from the target json")]
    [Trait("Category", "Unit")]
    public void JsonDeserializationSuccess()
    {
        // Arrange
        var created = DateTimeOffset.UtcNow;
        var reportItems = new List<HealthCheckReportItem>
        {
            new() {
                ResourceName = "Resource1",
                Status = ResourceStatus.Healthy
            },
            new() {
                ResourceName = "Resource2",
                Status = ResourceStatus.Unhealthy
            }
        };

        var json = $"{{\"timestamp\":\"{created:o}\",\"resources\":" +
            $"[{{\"resource_name\":\"Resource1\",\"resource_status\":\"Healthy\"}}," +
            $"{{\"resource_name\":\"Resource2\",\"resource_status\":\"Unhealthy\"}}]}}";

        // Act
        var deserializedReport = JsonConvert.DeserializeObject<HealthCheckReport>(json);

        // Assert
        deserializedReport.Should().NotBeNull();
        deserializedReport!.Created.Should().Be(created);
        deserializedReport.ReportItems.Should().BeEquivalentTo(reportItems);
    }
}
