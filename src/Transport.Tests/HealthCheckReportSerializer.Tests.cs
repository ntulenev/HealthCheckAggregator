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
}