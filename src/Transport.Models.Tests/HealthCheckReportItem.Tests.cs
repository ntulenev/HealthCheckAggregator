using Models;

using FluentAssertions;

using Newtonsoft.Json;

namespace Transport.Models.Tests;

public class HealthCheckReportItemTests
{
    [Theory(DisplayName = $"{nameof(HealthCheckReportItemTests)} can be serialized and deserialized")]
    [Trait("Category", "Unit")]
    [InlineData(ResourceStatus.Unhealthy)]
    [InlineData(ResourceStatus.Healthy)]
    public void JsonSerializationDeserializationSuccess(ResourceStatus status)
    {
        // Arrange
        var item = new HealthCheckReportItem
        {
            ResourceName = "Resource1",
            Status = status
        };

        // Act
        var json = JsonConvert.SerializeObject(item);
        var deserializedItem = JsonConvert.DeserializeObject<HealthCheckReportItem>(json);

        // Assert
        deserializedItem.Should().NotBeNull();
        deserializedItem!.ResourceName.Should().Be("Resource1");
        deserializedItem.Status.Should().Be(status);
    }

    [Fact(DisplayName = $"{nameof(HealthCheckReportItemTests)} can be deserialized from target json")]
    [Trait("Category", "Unit")]
    public void JsonDeserializationFromTargetJsonSuccess()
    {
        // Arrange
        var json = "{\"resource_name\":\"Resource2\",\"resource_status\":\"unhealthy\"}";

        // Act
        var deserializedItem = JsonConvert.DeserializeObject<HealthCheckReportItem>(json);

        // Assert
        deserializedItem.Should().NotBeNull();
        deserializedItem!.ResourceName.Should().Be("Resource2");
        deserializedItem.Status.Should().Be(ResourceStatus.Unhealthy);
    }
}
