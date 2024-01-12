using Newtonsoft.Json;

using Models;
using HealthCheckReport = Transport.Models.HealthCheckReport;
using HealthCheckReportItem = Transport.Models.HealthCheckReportItem;

namespace Transport.Tests;

public class HealthCheckReportSerializerTests
{
    [Fact(DisplayName = $"{nameof(HealthCheckReportSerializer)} can be created")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange + Act
        var exception = Record.Exception(()
            => _ = new HealthCheckReportSerializer());

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(HealthCheckReportSerializer)} can't serialize null data")]
    [Trait("Category", "Unit")]
    public void CannotSerializeNullData()
    {
        // Arrange
        var serializer = new HealthCheckReportSerializer();

        // Act
        var exception = Record.Exception(() => serializer.Serialize(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HealthCheckReportSerializer)} could serialize data")]
    [Trait("Category", "Unit")]
    public void CouldSerializeData()
    {
        // Arrange
        var testDate = DateTimeOffset.UtcNow;
        var serializer = new HealthCheckReportSerializer();
        var report = new HealthCheckReport
        {
            Created = testDate,
            ReportItems = new List<HealthCheckReportItem>
            {
                new()
                {
                    ResourceName = "Resource 1",
                    Status = ResourceStatus.Healthy
                },
                new()
                {
                    ResourceName = "Resource 2",
                    Status = ResourceStatus.Unhealthy
                }
            }
        };
        
        // Act
        var result = serializer.Serialize(report);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        var deserializedReport = JsonConvert.DeserializeObject<HealthCheckReport>(result)!;
        deserializedReport.Created.Should().Be(testDate);
        deserializedReport.ReportItems.Should().NotBeNullOrEmpty();
        deserializedReport.ReportItems.Should().HaveCount(2);
        deserializedReport.ReportItems.First().ResourceName.Should().Be("Resource 1");
        deserializedReport.ReportItems.First().Status.Should().Be(ResourceStatus.Healthy);
        deserializedReport.ReportItems.Last().ResourceName.Should().Be("Resource 2");
        deserializedReport.ReportItems.Last().Status.Should().Be(ResourceStatus.Unhealthy);
    }
}