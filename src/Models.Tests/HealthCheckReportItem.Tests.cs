using FluentAssertions;

namespace Models.Tests
{
    public class HealthCheckReportItemTests
    {
        [Fact(DisplayName = $"{nameof(HealthCheckReportItem)} can be created with valid params")]
        [Trait("Category", "Unit")]
        public void ConstructorInitializesResourceNameAndStatusProperties()
        {
            // Arrange
            var resourceName = new ResourceName("TestResource");
            var status = ResourceStatus.Healthy;

            // Act
            var reportItem = new HealthCheckReportItem(resourceName, status);

            // Assert
            reportItem.ResourceName.Should().Be(resourceName);
            reportItem.Status.Should().Be(status);
        }

        [Fact(DisplayName = $"{nameof(HealthCheckReportItem)} throws ArgumentNullException for null ResourceName")]
        [Trait("Category", "Unit")]
        public void ConstructorThrowsArgumentNullExceptionForNullResourceName()
        {
            // Arrange
            ResourceName resourceName = null!;
            var status = ResourceStatus.Healthy;

            // Act
            Action act = () => _ = new HealthCheckReportItem(resourceName, status);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = $"{nameof(HealthCheckReportItem)} throws ArgumentException for invalid Status")]
        [Trait("Category", "Unit")]
        public void ConstructorThrowsArgumentExceptionForInvalidStatus()
        {
            // Arrange
            var resourceName = new ResourceName("TestResource");
            var invalidStatus = (ResourceStatus)int.MaxValue;

            // Act
            Action act = () => _ =new HealthCheckReportItem(resourceName, invalidStatus);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
